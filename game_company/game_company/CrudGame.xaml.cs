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
    public partial class CrudGame : ContentPage
    {
        string apiUrl = "https://game-api-u9uv.onrender.com/v1/game";
        public CrudGame()
        {
            InitializeComponent();

        }
        private async void cmdUpdate_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtGameId.Text) || string.IsNullOrEmpty(txtGameName.Text) || string.IsNullOrEmpty(txtCategoryId.Text) || string.IsNullOrEmpty(txtDevId.Text))
                {
                    await DisplayAlert("Error", "ID del juego, Nombre del juego, ID de la categoría y ID del desarrollador son campos obligatorios", "OK");
                    return;
                }

                using (var wc = new WebClient())
                {
                    wc.Headers.Add("Content-Type", "application/json");

                    var game = new Game
                    {
                        game_id = int.Parse(txtGameId.Text),
                        cat_id = int.Parse(txtCategoryId.Text),
                        dev_id = int.Parse(txtDevId.Text),
                        game_name = txtGameName.Text,
                        game_date_release = DateTime.Parse(txtGameDateRelease.Text), // Asegúrate de tener el formato correcto
                        game_code_unique = txtGameCodeUnique.Text,
                        game_size = double.Parse(txtGameSize.Text), // Puedes cambiar esto según tus necesidades
                        game_price = double.Parse(txtGamePrice.Text) // Puedes cambiar esto según tus necesidades
                    };

                    var jsonGame = Newtonsoft.Json.JsonConvert.SerializeObject(game);

                    try
                    {
                        var respuesta = wc.UploadString($"{apiUrl}", "PUT", jsonGame);

                        if (!string.IsNullOrEmpty(respuesta))
                        {
                            await DisplayAlert("Éxito", "Juego actualizado correctamente", "OK");
                            LimpiarEntradas();
                        }
                        else
                        {
                            await DisplayAlert("Error", "Error al actualizar el juego", "OK");
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
                var url = $"{apiUrl}/{txtGameId.Text}";
                client.BaseAddress = new Uri(apiUrl);
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
                var resp = webClient.GetStringAsync($"{apiUrl}/{txtGameId.Text}");
                resp.Wait();

                var json = resp.Result;
                var data = JsonConvert.DeserializeObject<Game>(json);

                // Asigna los valores a las entradas
                txtGameId.Text = data.game_id.ToString();
                txtCategoryId.Text = data.cat_id.ToString();
                txtDevId.Text = data.dev_id.ToString();
                txtGameName.Text = data.game_name;
                txtGameDateRelease.Text = data.game_date_release.ToString("yyyy-MM-dd");
                txtGameCodeUnique.Text = data.game_code_unique;
                txtGameSize.Text = data.game_size.ToString();
                txtGamePrice.Text = data.game_price.ToString();
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
                    game_id = int.Parse(txtGameId.Text),
                    cat_id = int.Parse(txtCategoryId.Text),
                    dev_id = int.Parse(txtDevId.Text),
                    game_name = txtGameName.Text,
                    game_date_release = DateTime.Parse(txtGameDateRelease.Text),
                    game_code_unique = txtGameCodeUnique.Text,
                    game_size = decimal.Parse(txtGameSize.Text),
                    game_price = decimal.Parse(txtGamePrice.Text)
                    // Agrega aquí los demás campos según la estructura de tu JSON
                });

                var request = new HttpRequestMessage(HttpMethod.Post, apiUrl);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                var resp = webClient.SendAsync(request);
                resp.Wait();

                json = resp.Result.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<Game>(json);

                // Actualiza el ID u otros valores según la respuesta del servidor
                txtGameId.Text = data.game_id.ToString();
            }
        }

        private async void cmdRegresar_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private void LimpiarEntradas()
        {
            // Limpia el contenido de todas las entradas
            txtGameId.Text = string.Empty;
            txtCategoryId.Text = string.Empty;
            txtDevId.Text = string.Empty;
            txtGameName.Text = string.Empty;
            txtGameDateRelease.Text = string.Empty;
            txtGameCodeUnique.Text = string.Empty;
            txtGameSize.Text = string.Empty;
            txtGamePrice.Text = string.Empty;
            // Limpia aquí los demás campos según la estructura de tu JSON
        }
    }
}