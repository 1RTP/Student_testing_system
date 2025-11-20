using System.Collections.Generic;
public class Answer
{
    public int Id { get; set; }
    public string Text { get; set; }
    public bool IsCorrect { get; set; }
}
public class Question
{
    public int Id { get; set; }
    public string Text { get; set; }
    public List<Answer> Answers { get; set; } = new List<Answer>();
}
public class Test
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public List<Question> Questions { get; set; } = new List<Question>();
}