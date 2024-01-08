using System;
using System.IO;

namespace DesktopCleanup // Note: actual namespace depends on the project name.
{
    public class OrgDirs
    {
        public string desktopPath = @"c:\Users\SnoFlak\Desktop";

        public string mediaFolder = @"c:\Users\SnoFlak\Desktop\Media"; // Media Files
        public string musicFolder = @"c:\Users\SnoFlak\Desktop\Media\Music"; // .mp3, .ogg, .wav
        public string videoFolder = @"c:\Users\SnoFlak\Desktop\Media\Videos"; // .mp4
        public string pictureFolder = @"c:\Users\SnoFlak\Desktop\Media\Pictures"; // .png, .jpg, .svg, .gif
        public string exeFolder = @"c:\Users\SnoFlak\Desktop\Media\Exes"; // .exe

        public string miscFolder = @"c:\Users\SnoFlak\Desktop\Misc"; // Miscelaneous Files 
        public string txtFolder = @"c:\Users\SnoFlak\Desktop\Misc\Texts"; // .txt, .pdf
        public string folderFolder = @"c:\Users\SnoFlak\Desktop\Misc\Folders"; // folders
        public string zipsFolder = @"c:\Users\SnoFlak\Desktop\Misc\Zips"; // .7z, .rar, .zip
    }

    internal class Program
    {
        static string CheckExtension(string extension, OrgDirs orgDirs)
        {
            switch (extension)
            {
                case ".mp3":
                case ".ogg":
                case ".wav":
                    return orgDirs.musicFolder;
                case ".mp4":
                case ".wmv":
                case ".avi":
                    return orgDirs.videoFolder;
                case ".png":
                case ".jpg":
                case ".svg":
                case ".gif":
                    return orgDirs.pictureFolder;
                case ".exe":
                    return orgDirs.exeFolder;
                case ".txt":
                case ".pdf":
                    return orgDirs.txtFolder;
                case ".7z":
                case ".rar":
                case ".zip":
                    return orgDirs.zipsFolder;
                default:
                    return orgDirs.miscFolder;
            }
        }

        static void Main(string[] args)
        {

            OrgDirs orgDirs = new OrgDirs();

            string[] mediaFolders =
            {
                orgDirs.musicFolder,
                orgDirs.videoFolder,
                orgDirs.pictureFolder,
                orgDirs.exeFolder
            };

            string[] miscFolders =
            {
                orgDirs.txtFolder,
                orgDirs.folderFolder,
                orgDirs.zipsFolder
            };


            if (Directory.Exists(orgDirs.desktopPath)) // ensure you're not fucking crazy
            {
                // Check / Generate all folders for organization

                //ensure Media Folder exists
                if (!Directory.Exists(orgDirs.mediaFolder))
                {
                    Console.WriteLine($"Creating Folder: {orgDirs.mediaFolder}");
                    Directory.CreateDirectory(orgDirs.mediaFolder);
                }
                //ensure Misc Folder Exists
                if (!Directory.Exists(orgDirs.miscFolder))
                {
                    Console.WriteLine($"Creating Folder: {orgDirs.miscFolder}");
                    Directory.CreateDirectory(orgDirs.miscFolder);
                }

                //check Media Folder subfolders
                for (int i = 0; i <= mediaFolders.Length - 1; i++)
                {
                    if (!Directory.Exists(mediaFolders[i]))
                    {
                        Console.WriteLine($"Creating Folder: {mediaFolders[i]}");
                        Directory.CreateDirectory(mediaFolders[i]);
                    }
                }
                //check Misc Folder subfolders
                for (int i = 0; i <= miscFolders.Length - 1; i++)
                {
                    if (!Directory.Exists(miscFolders[i]))
                    {
                        Console.WriteLine($"Creating Folder: {miscFolders[i]}");
                        Directory.CreateDirectory(miscFolders[i]);
                    }
                }

                //Check all files sitting in Desktop. We only care about the main directory
                string[] fileSystemEntries = Directory.GetFileSystemEntries(orgDirs.desktopPath);

                foreach (var entry in fileSystemEntries)
                {

                    Console.WriteLine(entry);

                    if (entry == orgDirs.mediaFolder || entry == orgDirs.miscFolder) { continue; }

                    if (File.Exists(entry))
                    {
                        //relocate to proper folder
                        string fileExtension = Path.GetExtension(entry);
                        string fileName = Path.GetFileName(entry);
                        string selectedPath = CheckExtension(fileExtension, orgDirs);

                        if (selectedPath == "0") { continue; }

                        if (fileExtension != null || selectedPath != null || fileName != null)
                        {
                            string newPath = Path.Combine(selectedPath, fileName);
                            Console.WriteLine($"Moving {entry} to {newPath}");
                            File.Move(entry, newPath);
                        }
                    }
                    else if (Directory.Exists(entry))
                    {

                        //relocate to proper folder
                        string folderName = Path.GetFileName(entry);

                        if (mediaFolders.Contains(entry) || miscFolders.Contains(entry)) { continue; }

                        if (folderName != null)
                        {
                            string newPath = Path.Combine(orgDirs.folderFolder, folderName);
                            Console.WriteLine($"Moving {entry} to {newPath}");
                            try
                            {
                                Directory.Move(entry, newPath);
                            }
                            catch (Exception ex) { Console.WriteLine(ex); }
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Unknown entry type: {entry}");
                    }
                }

                Console.WriteLine("Finished Organizing Desktop. Be better next time you fuck!");
                Console.ReadLine();
            }

        }//Main
    }
}