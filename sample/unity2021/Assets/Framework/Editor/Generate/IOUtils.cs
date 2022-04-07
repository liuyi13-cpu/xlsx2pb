using System;
using System.IO;

public static class IOUtils
{
	public static bool IsDiskFull(Exception ex)
	{
		const int HR_ERROR_HANDLE_DISK_FULL = unchecked((int)0x80070027);
		const int HR_ERROR_DISK_FULL = unchecked((int)0x80070070);

		return ex.HResult == HR_ERROR_HANDLE_DISK_FULL 
			|| ex.HResult == HR_ERROR_DISK_FULL;
	}
	
	public static void CreateDirIfNotExist(string fillFullPath)
	{
		fillFullPath = fillFullPath.Replace("\\", "/");
		if (File.Exists(fillFullPath)) {
			return;
		}

		//判断路径中的文件夹是否存在
		string dirPath = fillFullPath.Substring(0, fillFullPath.LastIndexOf('/'));
		string[] paths = dirPath.Split('/');
		if (paths.Length > 1) {
			string path = paths[0];
			for (int i = 1; i < paths.Length; i++) {
				path += "/" + paths[i];
				if (!Directory.Exists(path)) {
					Directory.CreateDirectory(path);
				}
			}
		}
	}
	
	public static void CreateAllSubPath(string parentDir, string relativePath)
	{
		if (string.IsNullOrEmpty(relativePath)) { return; }
		string dirName = System.IO.Path.GetDirectoryName(relativePath);
		relativePath = dirName.Replace("\\", "/");
		string[] folders = relativePath.Split('/');
		DirectoryInfo dirInfo = new DirectoryInfo(parentDir);
		for (int i = 0; i < folders.Length; ++i) {
			if (string.IsNullOrEmpty(folders[i]))
				continue;
			dirInfo = dirInfo.CreateSubdirectory(folders[i]);
		}
	}
	
	public static void CopyFilesRecursively(DirectoryInfo source, DirectoryInfo target, bool overwrite, string extension = null)
	{
		foreach (DirectoryInfo dir in source.GetDirectories()) {
			if ((dir.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden && (dir.Attributes & FileAttributes.System) != FileAttributes.System)
				CopyFilesRecursively(dir, target.CreateSubdirectory(dir.Name), overwrite, extension);
		}
		foreach (FileInfo file in source.GetFiles()) {
			if (!string.IsNullOrEmpty(extension)) {
				if (string.Equals(file.Extension, extension, StringComparison.OrdinalIgnoreCase))
					file.CopyTo(Path.Combine(target.FullName, file.Name), overwrite);
			} else {
				file.CopyTo(Path.Combine(target.FullName, file.Name), overwrite);
			}
		}
	}
	
	/// <summary>
	/// 0=根目录 1=子目录 2=子目录平铺
	/// </summary>
	public static void CopyFilesRecursively(DirectoryInfo source, DirectoryInfo target, bool overwrite, string[] extension, int subDir)
	{
		if (subDir > 0)
		{
			foreach (DirectoryInfo dir in source.GetDirectories()) {
				if ((dir.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden &&
				    (dir.Attributes & FileAttributes.System) != FileAttributes.System)
				{
					if (subDir == 1)
					{
						CopyFilesRecursively(dir, target.CreateSubdirectory(dir.Name), overwrite, extension, subDir);
					}
					else
					{
						CopyFilesRecursively(dir, target, overwrite, extension, subDir);
					}
				}
			}
		}
		
		foreach (FileInfo file in source.GetFiles()) {
			if (extension != null) {
				bool enableCopy = false;
				int count = extension.Length;
				for (int i = 0; i < count; i++) {
					if (string.IsNullOrEmpty(extension[i])) { continue; }
					if (extension[i].EndsWith(file.Extension, StringComparison.OrdinalIgnoreCase)) {
						enableCopy = true;
						break;
					}
				}
				if (enableCopy) {
					file.CopyTo(Path.Combine(target.FullName, file.Name), overwrite);
				}
			} else {
				file.CopyTo(Path.Combine(target.FullName, file.Name), overwrite);
			}
		}
	}
}