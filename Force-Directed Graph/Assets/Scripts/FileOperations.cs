using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;
using graph;

public class FileOperations
{
    private static List<string> lines;

    public static bool LoadFile(string fileName, bool hasColumnNameLine)
    {
        try
        {
            fileName = Path.GetFullPath(fileName);
            int lineNumber = 1;
            lines = new List<string>();
            StreamReader stream = new StreamReader(fileName);

            while (!stream.EndOfStream)
            {
                string line = stream.ReadLine();

                if (lineNumber == 1 && hasColumnNameLine) {
                    lineNumber++;
                    continue;
                }

                lines.Add(line);
            }

            stream.Close();
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public static List<string> GetLines() {
        return lines;
    }

}
