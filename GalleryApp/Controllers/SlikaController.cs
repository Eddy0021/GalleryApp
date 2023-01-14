using GalleryApp.IRepository;
using GalleryApp.Models;
using GalleryApp.ResponeData;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GalleryApp.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class SlikaController : ControllerBase
    {
        ISlikaRepository _slikaRepository;

        public SlikaController(IWebHostEnvironment webHostEnvironment, ISlikaRepository slikaRepository)
        {
            
            _slikaRepository = slikaRepository;
        }      

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                List<Slika> slikeReturn = new List<Slika>();
                slikeReturn = await _slikaRepository.GetAllByAlbumID(id);

                return Ok(slikeReturn);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("UploadImage")]
        public async Task<IActionResult> Upload([FromForm]UploadSlikaResponse slika)
        {
            try
            {              
                var slikaReturn = await _slikaRepository.Upload(slika); //slikaReturn > Response

                return Ok(slikaReturn);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateSlikaResponse request)
        {
            try
            {
                var slikaReturn = await _slikaRepository.Update(request);

                return Ok(slikaReturn);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteImage/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var slikaReturn = await _slikaRepository.Delete(id);

                return Ok(slikaReturn);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
