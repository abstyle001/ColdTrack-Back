namespace ColdTrack_Back.Utils;

public static class AvatarUtil
{
    public static async Task<string?> UploadAvatar(string id, IFormFile? file, IConfiguration config, IWebHostEnvironment env)
    {
        if (file == null || file.Length == 0)
        {
            return null;
        }
        var originFileName = file.FileName;
        var extension = Path.GetExtension(originFileName);
        var fileName = id + extension;
        var avatarDir = Path.Combine(env.ContentRootPath, "Statics", "Avatars");
        if (!Directory.Exists(avatarDir))
        {
            Directory.CreateDirectory(avatarDir);
        }
        var filePath = Path.Combine(avatarDir, fileName);
        await using var fs = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(fs);
        return config["Static:Url"] + "avatars/" + fileName;
    }
}