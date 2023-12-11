using NRules.Fluent.Dsl;
using NRules.RuleModel;
using PlaninngResolver.Domain.Entities;

namespace PlaninngResolver.Domain.Application.Rules;

public class InvalidLectureRule : Rule
{
    IEnumerable<Lecture> planing = null;
    Tc tc = null;
    Dictionary<Guid?, int?> slot = null;

    public override void Define()
    {
        When()
            .Match(() => planing) // This line is added
            .Match(() => tc,
                tc => planing.Any(lecture =>
                    lecture.SectionId == tc.SectionId && lecture.Seance == slot.FirstOrDefault().Value));

        Then()
            .Do(ctx => UpdateFalse(ctx));
    }

    private void UpdateFalse(IContext ctx)
    {
        Console.WriteLine("InvalidLectureRule");
        ctx.Insert(new ValidLecture { Exist = false });
        tc = null;
    }
}