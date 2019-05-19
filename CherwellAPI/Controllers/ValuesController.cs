using System;
using Microsoft.AspNetCore.Mvc;
using CherwellModels.Models;


namespace CherwellAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private const int MINROW = 1;
        private const int MAXROW = 6;
        private const int MINCOL = 1;
        private const int MAXCOL = 12;
        private const int MINX= 0;
        private const int MAXX = 60;
        private const int MINY = 0;
        private const int MAXY = 60;

        // GET api/values/triangle/F/12
        [HttpGet("triangle/{row}/{col}")]
        public ActionResult<string> GetTriangle(char row, int col)
        {
            Triangle triangleResponse = new Triangle();
            try
            {
                int rowNumber = row - 'A' + 1;
                if (rowNumber < MINROW || rowNumber > MAXROW || col < MINCOL || col > MAXCOL)
                {
                    return BadRequest("Invalid parameter");
                }
                if (col % 2 != 0)  //odd column
                {
                    triangleResponse.Vertex1.x = (col - 1) * 5;
                    triangleResponse.Vertex1.y = rowNumber * 10;
                    triangleResponse.Vertex2.x = (col - 1) * 5;
                    triangleResponse.Vertex2.y = (rowNumber - 1) * 10;
                    triangleResponse.Vertex3.x = (col + 1) * 5;
                    triangleResponse.Vertex3.y = rowNumber * 10;
                }
                else   //even column
                {
                    triangleResponse.Vertex1.x = col * 5;
                    triangleResponse.Vertex1.y = (rowNumber - 1) * 10;
                    triangleResponse.Vertex2.x = (col - 2) * 5;
                    triangleResponse.Vertex2.y = (rowNumber - 1) * 10;
                    triangleResponse.Vertex3.x = col * 5;
                    triangleResponse.Vertex3.y = rowNumber * 10;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return Ok(triangleResponse);
        }

        [ProducesResponseType(typeof(Matrix), 200)]
        [HttpPost("vertex")]
        public ActionResult<string> Post([FromBody] Vertices request)
        {

            Matrix matrixResponse = new Matrix();
            bool inValid = true;
            try
            {

                if (!CheckVertex(request.Vertex1x,request.Vertex1y) | 
                    !CheckVertex(request.Vertex2x, request.Vertex2y) | 
                    !CheckVertex(request.Vertex3x, request.Vertex3y))
                {
                    return BadRequest("Invalid parameter");
                }

                int v1x = 0;
                int v1y = 0;
                int v2x = 0;
                int v2y = 0;
                int v3x = 0;
                int v3y = 0;

                for (int rowNumber = 1; rowNumber <= 6; rowNumber++)
                {
                    for (int col = 1; col <= 12; col++)
                    {

                        if (col % 2 != 0)
                        {
                            v1x = (col - 1) * 5;
                            v1y = rowNumber * 10;
                            v2x = (col - 1) * 5;
                            v2y = (rowNumber - 1) * 10;
                            v3x = (col + 1) * 5;
                            v3y = rowNumber * 10;
                        }
                        else
                        {
                            v1x = col * 5;
                            v1y = (rowNumber - 1) * 10;
                            v2x = (col - 2) * 5;
                            v2y = (rowNumber - 1) * 10;
                            v3x = col * 5;
                            v3y = rowNumber * 10;
                        }
                        Console.WriteLine(string.Format("Col {0} Row {1} v1x {2} v1y {3} v2x {4} v2y {5} v3x {6} v3y {7} ", col, rowNumber, v1x, v1y, v2x, v2y, v3x, v3y));

                        if (request.Vertex1x == v1x &&
                            request.Vertex1y == v1y &&
                            request.Vertex2x == v2x &&
                            request.Vertex2y == v2y &&
                            request.Vertex3x == v3x &&
                            request.Vertex3y == v3y)
                        {
                            matrixResponse.row = Convert.ToChar(64 + rowNumber).ToString();
                            matrixResponse.col = col;
                            inValid = false;
                            break;
                        }

                    }
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            if (inValid)
            {
                return BadRequest("Invalid parameter");
            }

            return Ok(matrixResponse);
        }
        private bool CheckVertex(int x, int y)
        {
            if (x < MINX || x > MAXX || y < MINY || y > MAXY || (x % 10 != 0) || (y % 10 != 0))
            {
                return false;
            }
            return true;
        }
    }
}
