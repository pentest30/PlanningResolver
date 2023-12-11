using NRules.Fluent.Dsl;
using NRules.RuleModel;
using PlaninngResolver.Domain.Entities;

namespace PlaninngResolver.Domain.Application.Rules;

public class ValidLectureRuleForClassSeance : Rule
{
    Tc tc = null;
    List<ClassSeance> classSeances = null;
    Dictionary<Guid?, int?> slot = null;
    
    public override void Define()
    {
       

        When()
            .Match<Tc>(() => tc)
            .Match<List<ClassSeance>>(() => classSeances,
                c => c.Any(cr => slot != null && cr.Seance == slot.FirstOrDefault().Value && cr.ClassRoomTypeId.Equals(tc.ClassRoomTypeId)));

        Then()
            .Do(ctx => Update(ctx));
    }
    private void Update(IContext context)
    {
        context.Insert(new ValidLecture { Exist = true });
        tc = null;
    }
}
