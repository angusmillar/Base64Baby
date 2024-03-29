﻿// See https://aka.ms/new-console-template for more information

System.IO.DirectoryInfo InputBase64 = new System.IO.DirectoryInfo(@"C:\temp\Base64Baby\Encoder Input");
SetupFolders(InputBase64);
System.IO.DirectoryInfo Base64Output = new System.IO.DirectoryInfo(@"C:\temp\Base64Baby\Encoder Output");
SetupFolders(Base64Output);
System.IO.DirectoryInfo Base64Input = new System.IO.DirectoryInfo(@"C:\temp\Base64Baby\Decoder Input");
SetupFolders(Base64Input);
System.IO.DirectoryInfo FileOutput = new System.IO.DirectoryInfo(@"C:\temp\Base64Baby\Decoder Output");
SetupFolders(FileOutput);

Console.WriteLine($"The Base64 encoders Input folder is  : {InputBase64.FullName}");
Console.WriteLine($"The Base64 encoders Output folder is : {Base64Output.FullName}");
Console.WriteLine();
Console.WriteLine($"The Base64 decoder Input folder is   : {Base64Input.FullName}");
Console.WriteLine($"The Base64 decoder Output folder is  : {FileOutput.FullName}");
Console.WriteLine();
Console.WriteLine("Base64 Baby is now running!");


int ErrorCount = 0;
bool Running = true;
while (Running)
{
    try
    {
        //Encoder
        FileInfo[] FileListEncoder = InputBase64.GetFiles();
        foreach (var Item in FileListEncoder)
        {
            byte[] AllBytes = ReadBytesFromFile(Item.FullName);
            WriteToBase64EncodedFile(AllBytes, Base64Output.FullName + @"\" + Item.Name);
            File.Delete(Item.FullName);
            Console.WriteLine($"Encoded file: {Item.Name}");
        }

        //Decoder
        FileInfo[] FileListDecoder = Base64Input.GetFiles();
        foreach (var Item in FileListDecoder)
        {
            string Data = ReadStringFromFile(Item.FullName);
            WriteToFileDecodeBase64(Data, FileOutput.FullName + @"\" + Item.Name);
            File.Delete(Item.FullName);
            Console.WriteLine($"Decoded file: {Item.Name}");
        }

        Thread.Sleep(500);
    }
    catch (Exception Exec)
    {
        ErrorCount = ErrorCount + 1;
        if (ErrorCount == 5)
        {
            Console.WriteLine($"An error has occured and has not cleared after 5 attempts.");
            Console.WriteLine("Error was:");
            Console.Write(Exec.Message);
            Console.WriteLine(" ");
            Console.WriteLine("Process has not stopped running.");
            Console.WriteLine("Hit any key to end.");
            Console.ReadKey();
            Running = false;
        }
    }
}


static void SetupFolders(DirectoryInfo Directory)
{
    if (!Directory.Exists)
    {
        Directory.Create();
    }
}


static byte[] ReadBytesFromFile(string filename)
{
    FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);

    // Create a byte array of file stream length
    byte[] Data = new byte[fs.Length];

    //Read block of bytes from stream into the byte array
    var Read = fs.Read(Data, 0, System.Convert.ToInt32(fs.Length));

    //Close the File Stream
    fs.Close();
    return Data; //return the byte data
}

static string ReadStringFromFile(string filename)
{
    return File.ReadAllText(filename);
}

static bool WriteToBase64EncodedFile(byte[] Bytes,
    string FilePath)
{
    var FileInfo = new FileInfo(FilePath);
    if (File.Exists(FilePath))
    {
        FilePath = FileInfo.DirectoryName + @"\" + DateTime.Now.ToString("yyymmddHHmmss ") + FileInfo.Name;
    }
    else
    {
        FilePath = FileInfo.DirectoryName + @"\" + FileInfo.Name;
    }

    File.WriteAllText(FilePath, Convert.ToBase64String(Bytes));
    return true;
}

static bool WriteToFileDecodeBase64(string Data,
    string FilePath)
{
    var FileInfo = new FileInfo(FilePath);
    if (File.Exists(FilePath))
    {
        //If the same file is already in the output folder then date time stamp this new file
        FilePath = FileInfo.DirectoryName + @"\" + DateTime.Now.ToString("yyymmddHHmmss ") + FileInfo.Name;
    }
    else
    {
        FilePath = FileInfo.DirectoryName + @"\" + FileInfo.Name;
    }

    File.WriteAllBytes(FilePath, Convert.FromBase64String(Data));
    return true;
}