namespace IN12B8_WindowsService.CoreLogic;

public static class TxtHandler
{
    public static void SaveTxt(string path, string content)
    {
        if (!path.EndsWith(".txt"))
        {
            path += ".txt";
        }
        using StreamWriter sw = File.CreateText(path);
        sw.Write(content);
        sw.Close();
    }

    public static string ReadTxt(string path)
    {
        if (!path.EndsWith(".txt"))
        {
            path += ".txt";
        }

        path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);

        if (!Path.Exists(path))
        {
            return "";
        }
        
        StreamReader r = new StreamReader(path);
        string txt = r.ReadToEnd();
        r.Close();
        return txt;
    }
}