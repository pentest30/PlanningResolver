using NRules.Fluent.Dsl;
using NRules.RuleModel;
using PlaninngResolver.Domain.Entities;

namespace PlaninngResolver.Domain.Application.Rules;

public class ValidLectureRule : Rule
{
    IEnumerable<Lecture> planing = null;
    Tc tc = null;
    Dictionary<Guid?, int?> slot = null;
    public override void Define()
    {
       

        When()
            .Match(() => planing) // This line is added
            .Match(() => tc,
                tc => !planing.Any(lecture => lecture.SectionId == tc.SectionId && lecture.Seance == slot.FirstOrDefault().Value));

        Then()
            .Do(ctx =>  Update(ctx) );
       
    }

   

    private void Update(IContext context)
    {
        context.Insert(new ValidLecture { Exist = true });
        tc = null;
    }
}

public class ValidLecture
{
    public bool Exist { get; init; }
}