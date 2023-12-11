using NRules.Fluent.Dsl;
using PlaninngResolver.Domain.Entities;

namespace PlaninngResolver.Domain.Application.Rules;

public class ValidLectureRuleForTeacher : Rule
{
    public override void Define()
    {
        IEnumerable<Lecture> planing = null;
        Tc tc = null;
        Dictionary<Guid?, int?> slot = null;


        When()
            .Match<IEnumerable<Lecture>>(() => planing) // Assuming planing is a list of Lecture
            .Match<Tc>(() => tc,
                tc => !planing.Any(lecture =>
                    lecture.TeacherId == tc.TeacherId && lecture.Seance == slot.FirstOrDefault().Value));

        Then()
            .Do(ctx => ctx.Insert(new ValidLecture() {Exist = true}));
    }
}