using System;
using System.Collections.Generic;
using System.IO;

namespace Assignment1
{
  

    public class DirWalker
    {

        public List<string> walk(String path, List<string> fileList)
        {

            string[] list = Directory.GetDirectories(path);


            if (list == null) return null;

            foreach (string dirpath in list)
            {
                if (Directory.Exists(dirpath))
                {
                    walk(dirpath, fileList);
                    //Console.WriteLine("Dir:" + dirpath);
                }
            }
            string[] files = Directory.GetFiles(path);
            foreach (string filepath in files)
            {
                if (filepath.Contains(".csv"))
                {
                    //Console.WriteLine("File:" + filepath);
                    fileList.Add(filepath);
                }
            }
            return fileList;
        }

//        public static void Main(String[] args)
//        {
//            DirWalker fw = new DirWalker();
//           fw.walk(@"/");
//        }

    }
}
