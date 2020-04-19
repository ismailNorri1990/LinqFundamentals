using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduction
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"C:\Windows";
            ShowLargesFilesWithLinq(path);
            Console.WriteLine("***");
            ShowLargesFilesWithoutLinq(path);
        }
        
        private static void ShowLargesFilesWithLinq(String path){
            /*var query = from file in new DirectoryInfo(path).GetFiles()
                        orderby file.Length descending
                        select file;*/

            var query = new DirectoryInfo(path).GetFiles().OrderByDescending(x => x.Length).Take(5);

            foreach (var file in query/*.Take(5)*/)
	{
                Console.WriteLine($"{file.Name, -20} : {file.Length, 10:N0}");
	}
        }

        private static void ShowLargesFilesWithoutLinq(String path)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(path);

            FileInfo[] files = directoryInfo.GetFiles();

            Array.Sort(files,new FileInfoComparer());

            for (int i = 0; i < 5; i++)
            {
                FileInfo file = files[i];
                Console.WriteLine($"{file.Name, -20} : {file.Length, 10:N0}");
            }

            Console.ReadLine();
        }
    }

    public class FileInfoComparer : IComparer<FileInfo>
    {
        public int Compare(FileInfo x, FileInfo y)
        {
            return y.Length.CompareTo(x.Length);
        }
    }
}
