using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CherwellModels.Models;
using System.Net.Http;
using Newtonsoft.Json;

namespace CherwellWeb.Pages
{
    public class TrianglesModel : PageModel
    {
        public Vertices VerticesRequest { get; set; }

        private static HttpClient Client = new HttpClient();
        private string httpURL = @"https://localhost:5001/";

        public async Task<IActionResult> OnGetTriangle([FromQuery]char row, int col)
        {
            Triangle triangleResponse = new Triangle();

            HttpResponseMessage result = await Client.GetAsync(httpURL + "api/Values/triangle/" + row + '/' + col);
            if (result.IsSuccessStatusCode == true)
            {
                string res = await result.Content.ReadAsStringAsync();
                triangleResponse = JsonConvert.DeserializeObject<Triangle>(res);
                return new JsonResult(triangleResponse);
            }
            return BadRequest("Error Occurred");

        }

        public async Task<IActionResult> OnPost(Vertices request)
        {
            Matrix matrixResponse = new Matrix();

            HttpResponseMessage result = await Client.PostAsync(httpURL + "api/Values/vertex", new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json"));
            if (result.IsSuccessStatusCode == true)
            {
                string res = await result.Content.ReadAsStringAsync();
                matrixResponse = JsonConvert.DeserializeObject<Matrix>(res);
                return new JsonResult(matrixResponse);
            }

            return BadRequest("Error Occurred");

        }
    }
}