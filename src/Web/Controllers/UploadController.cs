using prof_tester_api.src.Domain.IRepository;
using prof_tester_api.src.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using webApiTemplate.src.App.IService;

namespace prof_tester_api.src.Web.Controllers
{
    [ApiController]
    [Route("api/upload")]
    public class UploadController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ILecternRepository _lecternRepository;
        private readonly IJwtService _jwtService;

        public UploadController(
            IUserRepository userRepository,
            ILecternRepository lecternRepository,
            IJwtService jwtService
            )
        {
            _userRepository = userRepository;
            _lecternRepository = lecternRepository;
            _jwtService = jwtService;
        }

        [HttpPost("profileIcon"), Authorize]
        [SwaggerOperation("Загрузить иконку профиля")]
        [SwaggerResponse(200, Description = "Успешно", Type = typeof(string))]

        public async Task<IActionResult> UploadProfileIcon([FromHeader(Name = "Authorization")] string token)
        {
            if (Request.Form.Files.Count == 0)
                return BadRequest();

            var file = Request.Form.Files[0];
            var tokenPayload = _jwtService.GetTokenPayload(token);

            var filename = await FileUploader.UploadFileAsync(Constants.localPathToProfileIcons, file.OpenReadStream(), ".jpeg");
            await _userRepository.UpdateProfileIconAsync(tokenPayload.UserId, filename);
            return Ok(new { filename });
        }

        [HttpGet("profileIcon/{filename}")]
        [SwaggerOperation("Получить иконку профиля")]
        [SwaggerResponse(200, Description = "Успешно", Type = typeof(File))]
        [SwaggerResponse(404, Description = "Неверное имя файла")]

        public async Task<IActionResult> GetProfileIcon(string filename)
        {
            var bytes = await FileUploader.GetStreamFileAsync(Constants.localPathToProfileIcons, filename);
            if (bytes == null)
                return NotFound();

            return File(bytes, $"image/jpeg", filename);
        }

        [HttpGet("lectern/{filename}")]
        [SwaggerOperation("Получить иконку профиля")]
        [SwaggerResponse(200, Description = "Успешно", Type = typeof(File))]
        [SwaggerResponse(404, Description = "Неверное имя файла")]

        public async Task<IActionResult> GetLecternFileAsync(string filename)
        {
            var bytes = await FileUploader.GetStreamFileAsync(Constants.localPathToLecternFile, filename);
            if (bytes == null)
                return NotFound();

            return File(bytes, $"image/jpeg", filename);
        }

        [HttpPost("lectern/{lecternId}"), Authorize]
        [Consumes("multipart/form-data")]
        [SwaggerOperation("Загрузить иконку профиля как form-data")]
        [SwaggerResponse(200, Description = "Успешно")]
        [SwaggerResponse(400, Description = "Не прикреплены файлы")]
        [SwaggerResponse(404, Description = "Неверный идентификатор")]

        public async Task<IActionResult> UploadLecternFile(
            [FromHeader(Name = "Authorization")] string token,
            Guid lecternId)
        {
            if (Request.Form.Files.Count == 0)
                return BadRequest();

            var file = Request.Form.Files[0];
            var fileExtension = Path.GetExtension(file.FileName);
            var lectern = await _lecternRepository.GetAsync(lecternId);
            if (lectern == null)
                return NotFound();

            var filename = await FileUploader.UploadFileAsync(Constants.localPathToLecternFile, file.OpenReadStream(), fileExtension);
            await _lecternRepository.UpdateFilenameAsync(lecternId, filename);
            return Ok();
        }


    }
}