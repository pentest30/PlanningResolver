using NRules.Fluent.Dsl;
using NRules.RuleModel;
using PlaninngResolver.Domain.Entities;

namespace PlaninngResolver.Domain.Application.Rules;

public class InvalidLectureRuleForTeacher : Rule
{
    Tc tc = null;
    public override void Define()
    {
        IEnumerable<Lecture> planing = null;
        
        Dictionary<Guid?, int?> slot = null;


        When()
            .Match<IEnumerable<Lecture>>(() => planing) // Assuming planing is a list of Lecture
            .Match<Tc>(() => tc,
                tc => planing.Any(lecture =>
                    lecture.TeacherId == tc.TeacherId && lecture.Seance == slot.FirstOrDefault().Value));

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