namespace Homeschool.DataTests.Context;

using System;

using Data.Context;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Newtonsoft.Json;

using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.Extensions.Logging;

using Xunit;

public class HomeschoolDataImporter
{
    public HomeschoolDataImporter()
    {
        var builder = Host.CreateDefaultBuilder();
        builder.ConfigureServices(
            collection =>
            {
                collection.AddSingleton(
                        provider =>
                        {
                            string cs
                                = provider.GetRequiredService<IConfiguration>()[
                                    "ConnectionString"];

                            return new DbContextOptionsBuilder<HomeschoolContext>()
                                .EnableDetailedErrors(true)
                                .UseSqlServer(
                                    cs, _ =>
                                    {
                                    })
                                .Options;
                        }
                    )
                    .AddDbContext<HomeschoolContext>();
            }
        );

        BuiltHost = builder.Build();
    }

    private IHost BuiltHost { get; }

    [Theory, InlineData("C:\\GitHub\\Homeschool\\Courses.json")]
    public void ImportFromJson(string jsonFilename)
    {
        if (!File.Exists(jsonFilename))
        {
            return;
        }

        string json = File.ReadAllText(jsonFilename);

        var ctx = BuiltHost.Services.GetRequiredService<HomeschoolContext>();

        var courses = JsonConvert.DeserializeObject<Course[]>(json);
        var hsCourses = ctx.HsCourses.Include("HsChapters")
            .Include("HsChapters.HsLessons")
            .ToList();

        foreach (var course in hsCourses)
        {
            var matchedCourse = courses.FirstOrDefault(
                co => course.CourSlug.Equals(
                    co.slug.Replace("-", " "),
                    StringComparison.OrdinalIgnoreCase
                )
            );

            if (matchedCourse is null)
            {
                continue;
            }

            int chapterOrder = 0;
            var groups = matchedCourse.Lessons.GroupBy(l => l.chapterTitle);
            var titleRegex = new Regex(@"Ch\.?\s(\d)\.\s");
            var orderedChapters = course.HsChapters.Select(
                c =>
                new {
                    c, OrderByTitle=titleRegex.Replace(c.ChapTitle, $"Ch 0$1. ")
                }
            ).OrderBy(c=>c.OrderByTitle).ToList();
            foreach (var obchapter in orderedChapters)
            {
                var chapter = obchapter.c;
                chapter.ChapDisplayOrder = chapterOrder++;
                while (chapter.ChapSlug.IndexOf("  ", StringComparison.Ordinal) > -1)
                {
                    chapter.ChapSlug = chapter.ChapSlug.Replace("  ", " ");
                }
                var regex = new Regex(@"Ch\.?\s\d+\.\s");
                var matchedChapter = groups.FirstOrDefault(
                    g => regex.Replace(g.Key, "").EndsWith(chapter.ChapSlug, StringComparison.OrdinalIgnoreCase)
                );

                if (matchedChapter is null)
                {
                    continue;
                }

                chapter.ChapTitle = matchedChapter.Key;

                int lessonOrder = 0;
                var lessonRegex = new Regex(@"Lesson\s(\d)\s");
                var orderedLessons = chapter.HsLessons.Select(
                        l => new
                        {
                            l,
                            OrderByTitle = lessonRegex.Replace(l.LessTitle, $"Lesson 0$1 ")
                        }
                    )
                    .OrderBy(c => c.OrderByTitle)
                    .ToList();

                foreach (var oblesson in orderedLessons)
                {
                    var lesson = oblesson.l;
                    lesson.LessDisplayOrder = lessonOrder++;
                    var matchedLesson = matchedChapter.FirstOrDefault(
                        l => l.lessonTitle.Replace("&amp;", "&").EndsWith(
                            lesson.LessSlug,
                            StringComparison.OrdinalIgnoreCase
                        )
                    );

                    if (matchedLesson is null)
                    {
                        continue;
                    }

                    lesson.LessTitle = matchedLesson.lessonTitle;
                    lesson.LessUrl = matchedLesson.lessonUrl;
                }

                var rows = ctx.SaveChanges();
                var expected = matchedChapter.Count() + 1;

                BuiltHost.Services.GetService<ILogger<HomeschoolDataImporter>>()!
                    .LogInformation($"Chapter: {chapter.ChapTitle}: Saved {rows} of expected {expected}");
            }
        }
    }
}


public class Course
{
    public string slug { get; set; }
    public Lesson[] Lessons { get; set; }
}

public class Lesson
{
    public string courseSlug { get; set; }
    public string chapterTitle { get; set; }
    public string lessonTitle { get; set; }
    public string lessonUrl { get; set; }
}
