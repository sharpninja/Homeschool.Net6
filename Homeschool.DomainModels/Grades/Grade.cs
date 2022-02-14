namespace Homeschool.DomainModels.Grades;

public record Grade(AssessmentGrade Assessment)
{
    public string? LetterGrade =>
        Assessment.EarnedPoints is not null && Assessment.TotalPoints is not null && Assessment.TotalPoints > 0
            ? ((decimal)Assessment.EarnedPoints /
               (decimal)Assessment.TotalPoints) switch
            {
                >= .925m => "A",
                >= .845m => "B",
                >= .755m => "C",
                >= .695m => "D",
                _ => "F",
            }
            : null;

    public override string ToString()
        => LetterGrade;
}
