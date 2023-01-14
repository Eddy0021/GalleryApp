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
    public class AlbumController : ControllerBase
    {
        IAlbumRepository _albumRepository;

        public AlbumController(IAlbumRepository albumRepository)
        {
            _albumRepository = albumRepository;
        }

        [HttpGet]
        [Route("GetAlbum/{id}")]
        public async Task<IActionResult> Get(int id)
        {       
            try
            {
                List<Album> albumsReturn = new List<Album>();
                albumsReturn = await _albumRepository.GetAlbumByUserId(id);

                return Ok(albumsReturn);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> Create(UploadAlbumResponse album)
        {
            try
            {
                Album albumRet = await _albumRepository.Create(album);
                return Ok(albumRet);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateAlbumResponse request)
        {
            try
            {
                Album albumRet = await _albumRepository.Update(request);
                return Ok(albumRet);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var albumRet = await _albumRepository.Delete(id);
                return Ok(albumRet);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
