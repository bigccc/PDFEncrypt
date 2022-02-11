// See https://aka.ms/new-console-template for more information

using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;

string pwd = Console.ReadLine();
string path = @"C:\ossbook";
DirectoryInfo root = new DirectoryInfo(path);
FileInfo[] files = root.GetFiles();
// Console.WriteLine(files[0].Extension);
foreach (var file in files)
{
    if (file.Extension == ".pdf")
    {
        try
        {
            deletePDFEncrypt(file.FullName, @"C:\newossbook\" + file.Name, pwd);
            file.Delete();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        Console.WriteLine(file.Name + "处理完成");
    }
    else
    {
        file.Delete();
        Console.WriteLine(file.Name + "已删除");
    }
}
// Console.WriteLine(files.Length);

// deletePDFEncrypt(srcPath, descPath);


/// <summary>
/// 将去掉PDF的加密
/// </summary>
/// <param name="sourceFullName">源文件路径(如：D:\old.pdf)</param>
/// <param name="newFullName">目标文件路径(如:D:\new.pdf)</param>
void deletePDFEncrypt(string sourceFullName, string newFullName, string pwd)
{
    if (string.IsNullOrEmpty(sourceFullName) || string.IsNullOrEmpty(newFullName))
    {
        throw new Exception("源文件路径或目标文件路径不能为空或null.");
    }

    //Console.WriteLine("读取PDF文档");
    try
    {
        // 创建一个PdfReader对象
        PdfReader reader = new PdfReader(sourceFullName, Encoding.Default.GetBytes(pwd));
        PdfReader.unethicalreading = true;
        // 获得文档页数
        int n = reader.NumberOfPages;
        // 获得第一页的大小
        Rectangle pagesize = reader.GetPageSize(1);
        float width = pagesize.Width;
        float height = pagesize.Height;
        // 创建一个文档变量
        Document document = new Document(pagesize, 50, 50, 50, 50);
        // 分割并创建该文档
        PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(newFullName, FileMode.Create));
        writer.SetEncryption(Encoding.Default.GetBytes("******"), Encoding.Default.GetBytes("******"), 0,
            PdfWriter.ENCRYPTION_AES_256);
        // 打开文档
        document.Open();
        // 添加内容
        PdfContentByte cb = writer.DirectContent;
        int i = 0;
        int p = 0;
        while (i < n)
        {
            document.NewPage();
            p++;
            i++;
            PdfImportedPage page1 = writer.GetImportedPage(reader, i);
            cb.AddTemplate(page1, 1f, 0, 0, 1f, 0, 0);
        }

        // 关闭文档
        document.Close();
        reader.Close();
    }
    catch (Exception ex)
    {
        throw new Exception(ex.Message);
    }
}