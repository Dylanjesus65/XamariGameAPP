using game_company.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace game_company
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CrudDeveloper : ContentPage
    {
        string apiUrl = "https://game-api-u9uv.onrender.com/v1/developer";
        public CrudDeveloper()
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

                using (var wc = new WebClient())
                {
                    wc.Headers.Add("Content-Type", "application/json");

                    var developer = new Developer
                    {
                        dev_id = int.Parse(txtId.Text),
                        dev_name = txtNombre.Text,
                        dev_country = txtCountry.Text,
                        dev_unique_code = txtUniqueCode.Text
                    };

                    var jsonDeveloper = Newtonsoft.Json.JsonConvert.SerializeObject(developer);

                    try
                    {
                        var respuesta = wc.UploadString($"{apiUrl}", "PUT", jsonDeveloper);

                        if (!string.IsNullOrEmpty(respuesta))
                        {
                            await DisplayAlert("Éxito", "Desarrollador actualizado correctamente", "OK");
                            LimpiarEntradas();
                        }
                        else
                        {
                            await DisplayAlert("Error", "Error al actualizar el desarrollador", "OK");
                        }
                    }
                    catch (WebException ex)
                    {
                        var response = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        await DisplayAlert("Error", $"Error de conexión: {ex.Message}\nRespuesta del servidor: {response}", "OK");
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
                var data = JsonConvert.DeserializeObject<Developer>(json);

                // Asigna los valores a las entradas
                txtId.Text = data.dev_id.ToString();
                txtNombre.Text = data.dev_name;
                txtCountry.Text = data.dev_country;
                txtUniqueCode.Text = data.dev_unique_code;
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
                    dev_id = int.Parse(txtId.Text),
                    dev_name = txtNombre.Text,
                    dev_country = txtCountry.Text,
                    dev_unique_code = txtUniqueCode.Text
                });

                var request = new HttpRequestMessage(HttpMethod.Post, apiUrl);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                var resp = webClient.SendAsync(request);
                resp.Wait();

                json = resp.Result.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<Developer>(json);

                // Actualiza el ID u otros valores según la respuesta del servidor
                txtId.Text = data.dev_id.ToString();
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