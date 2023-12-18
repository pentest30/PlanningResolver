using System.Globalization;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PlaninngResolver.Domain.Application.TimeTable;
using PlaninngResolver.Domain.Entities;
using PlaninngResolver.Domain.Infrastructure.Persistence;
using PlaninngResolver.Domain.Infrastructure.Persistence.Repositories;
using PlaninngResolver.Domain.Interfaces;

class Program
{
    static void Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
         var serviceProvider = new ServiceCollection()
            .AddSingleton(BuildConfiguration())
            .AddDbContext<PlanningDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("AccessDb")))
            .AddScoped(typeof(IRepository<>), typeof(Repository<>))
            .AddScoped(typeof(IResolverService), typeof(ResolverService))
            // Add other services
            .BuildServiceProvider();

        var resolverService = serviceProvider.GetService<IResolverService>();
        resolverService?.GeneratingPlanning(1, 1, 1);
        var repo = serviceProvider.GetService<IRepository<Lecture>>();
        var lectures = repo
            .GetAll()
            .Include(x => x.Teacher)
            .Include(x => x.Course)
            .Include(x => x.ClassRoom)
            .Include(x => x.Section)
            .Include(x => x.Groupe).ToList();
            CreateSchedulePdf(lectures, Path.GetTempPath() + "emploi.pdf");
       
    }

    private static IConfiguration BuildConfiguration()
    {
        return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
    }
    private static void CreateSchedulePdf(List<Lecture> entries, string filePath)
    {
        var formatCalendar = ProcessLectures(entries);
        Document doc = new Document(PageSize.A2,5,5,5,5);
        PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));
        doc.Open();
        PdfPTable table = new PdfPTable(7);
        table.HeaderRows = 1;
        table.SplitLate = false;
        table.SplitRows = true;
        table.AddCell("Jours");
        for (int i = 1; i <= 6; i++)
        {
            table.AddCell("Séance " + i);
        }
        // Add headers
        foreach (var dayEntry in formatCalendar)
        {
            table.AddCell(dayEntry.Key); 
            PdfPCell[] seanceCells = new PdfPCell[6];
            for (int i = 0; i < seanceCells.Length; i++)
            {
                seanceCells[i] = new PdfPCell(new Phrase(string.Empty)) { Border = Rectangle.BOTTOM_BORDER, BorderWidthBottom = 2f };
            }

            // Fill in the seance cells
            for (var index = 0; index < dayEntry.Value.Count; index++)
            {
                var seance = dayEntry.Value[index];
                seanceCells[index].Phrase = new Phrase(seance);
                seanceCells[index].Border = Rectangle.BOX;
            }

            // Add seance cells to the table
            foreach (var cell in seanceCells)
            {
                cell.Border = Rectangle.BOX;
                cell.BorderColor = BaseColor.BLACK; 
                table.AddCell(cell);
                
            }
        }

        doc.Add(table);
        doc.Close();
    }
    private static Dictionary<string, List<string>> ProcessLectures(List<Lecture> result)
    {
        var final = new List<Lecture>();
        var dictionary = new Dictionary<string, List<string>>();

        foreach (var lectureGroup in result.GroupBy(w => w.Seance))
        {
            var display = ConstructDisplayString(lectureGroup);
            var firstLecture = lectureGroup.FirstOrDefault();
        
            if (firstLecture != null)
            {
                Lecture lec = firstLecture.ShallowClone();
                lec.Display = display;
                final.Add(lec);
            }
        }
        var dayNames = CultureInfo.CurrentCulture.DateTimeFormat.DayNames;
        dictionary.Add(dayNames[6], ConstructList(final.Where(w => w.Seance <= 6).ToList(), 1));
        dictionary.Add(dayNames[0], ConstructList(final.Where(w => w.Seance >= 7 && w.Seance < 13).ToList(), 7));
        dictionary.Add(dayNames[1], ConstructList(final.Where(w => w.Seance >= 13 && w.Seance < 19).ToList(), 13));
        dictionary.Add(dayNames[2], ConstructList(final.Where(w => w.Seance >= 19 && w.Seance < 25).ToList(), 19));
        dictionary.Add(dayNames[3], ConstructList(final.Where(w => w.Seance >= 25 && w.Seance < 31).ToList(), 25));
        dictionary.Add(dayNames[4], ConstructList(final.Where(w => w.Seance >= 31).ToList(), 31));

        return dictionary;
    }

    private static string ConstructDisplayString(IEnumerable<Lecture> lectureGroup)
    {
        var stringBuilder = new StringBuilder();
        bool isFirst = true;

        foreach (var lecture in lectureGroup)
        {
            if (!isFirst)
            {
                stringBuilder.AppendLine("\n.................................");
            }

            if (lecture.Course.Name != null)
            {
                stringBuilder.AppendLine(lecture.Course.Name.ToUpper());
            }
            stringBuilder.AppendLine(lecture.ClassRoom.Name);
            stringBuilder.AppendLine(lecture.Teacher.Nom);
            stringBuilder.AppendLine(lecture.Section.Name);
            if (lecture.Groupe!= null)
            {
                stringBuilder.AppendLine(lecture.Groupe.Code);
            }

            isFirst = false;
        }

        return stringBuilder.ToString();
    }

    private static List<string> ConstructList(List<Lecture> lectures, int dayNumber)
    {
        var result = new List<string>();
        for (int i = 0; i < 6; i++)
        {
            var item = lectures.FirstOrDefault(w => w.Seance == dayNumber);
            result.Add((item != null ? item.Display : "") ?? throw new InvalidOperationException());
            dayNumber ++;
        }
        return result;
    }

}