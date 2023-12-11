using NRules.RuleModel;
using PlaninngResolver.Domain.Entities;

namespace PlaninngResolver.Domain.Application.Rules;

using NRules.Fluent.Dsl;
public class ValidLectureRuleForGroup : Rule
{
    Tc tc = null;
    IEnumerable<Lecture> planing = null;
       
    List<ClassSeance> classSeance = null;
    Dictionary<Guid?, int?> slot = null;
    

    public override void Define()
    {
        Func<Lecture, bool> lecturePredicate = (lecture) =>
            lecture.GroupeId == null &&
            lecture.SectionId == tc.SectionId &&
            lecture.Seance == slot.FirstOrDefault().Value;
        Func<Lecture, bool> lectureSectionPredicate = (lecture) =>
            lecture.GroupeId.HasValue &&
            lecture.GroupeId == tc.GroupeId && 
            lecture.Seance == slot.FirstOrDefault().Value;
        When()
            .Match(() => planing)
            .Match<Tc>(() => tc, tc => tc.GroupeId != null)
            .Match<IEnumerable<Lecture>>(planing => !planing.Any(lecturePredicate) && !planing.Any(lectureSectionPredicate)) ;

        Then()
            .Do(ctx => Update(ctx));
    }

    private void Update(IContext context)
    {
        context.Insert(new ValidLecture { Exist = true });
        tc = null;
    }
}