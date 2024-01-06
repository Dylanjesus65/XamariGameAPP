using game_company.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace game_company
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CrudCategory : ContentPage
    {
        string apiUrl = "https://game-api-u9uv.onrender.com/v1/category";
        public CrudCategory()
        {
            InitializeComponent();
        }

        private async void cmdUpdate_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtId.Text) || string.IsNullOrEmpty(txtNombre.Text))
                {
                    await DisplayAlert("Error", "ID y Nombre son campos obligatorios", "OK");
                    return;
                }

                using (var client = new HttpClient())
                {
                    var url = $"{apiUrl}/{txtId.Text}";
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));

                    var categoria = new Category
                    {
                        cat_id = int.Parse(txtId.Text),
                        cat_name = txtNombre.Text
                    };

                    var jsonCategoria = JsonConvert.SerializeObject(categoria);
                    var content = new StringContent(jsonCategoria, Encoding.UTF8, "application/json");

                    var resp = await client.PutAsync(url, content);

                    if (resp.IsSuccessStatusCode)
                    {
                        await DisplayAlert("Éxito", "Categoría actualizada correctamente", "OK");
                        LimpiarEntradas();
                    }
                    else
                    {
                        await DisplayAlert("Error", "Error al actualizar la categoría", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error: {ex.Message}", "OK");
            }
        }




        private void cmdDelete_Clicked(object sender, EventArgs e)
        {
            using (var client = new HttpClient())
            {
                var url = $"{apiUrl}/{txtId.Text}";
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));

                var resp = client.DeleteAsync(url);
                resp.Wait();

                LimpiarEntradas();
            }
        }

        private void cmdReadOne_Clicked(object sender, EventArgs e)
        {
            using (var webClient = new HttpClient())
            {
                var resp = webClient.GetStringAsync($"{apiUrl}/{txtId.Text}");
                resp.Wait();

                var json = resp.Result;
                var data = JsonConvert.DeserializeObject<Category>(json);

                // Asigna los valores a las entradas
                txtId.Text = data.cat_id.ToString();
                txtNombre.Text = data.cat_name;
                // Asigna aquí los demás valores según la estructura de tu JSON
            }
        }

        private void cmdInsert_Clicked(object sender, EventArgs e)
        {
            using (var webClient = new HttpClient())
            {
                webClient.BaseAddress = new Uri(apiUrl);
                webClient.DefaultRequestHeaders.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));

                var json = JsonConvert.SerializeObject(new
                {
                    cat_id = int.Parse(txtId.Text),
                    cat_name = txtNombre.Text
                    // Agrega aquí los demás campos según la estructura de tu JSON
                });

                var request = new HttpRequestMessage(HttpMethod.Post, apiUrl);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                var resp = webClient.SendAsync(request);
                resp.Wait();

                json = resp.Result.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<Category>(json);

                // Actualiza el ID u otros valores según la respuesta del servidor
                txtId.Text = data.cat_id.ToString();
            }
        }

        private async void cmdRegresar_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private void LimpiarEntradas()
        {
            // Limpia el contenido de todas las entradas
            txtId.Text = string.Empty;
            txtNombre.Text = string.Empty;
            // Limpia aquí los demás campos según la estructura de tu JSON
        }
    }
}