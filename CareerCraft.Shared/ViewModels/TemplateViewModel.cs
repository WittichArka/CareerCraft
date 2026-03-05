namespace CareerCraft.Shared.ViewModels;

public class TemplateViewModel
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string JobTitle { get; set; } = string.Empty;
    public List<PersonalInfo> Infos { get; set; } = [];

    public List<SoftSkill> SoftSkills { get; set; } = [];

    public List<Section> Sections { get; set; } = [];

    public class PersonalInfo
    {
        public int Order { get; set; } = 0;
        public string ImagePath { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public string CustomCssClass { get; set; } = string.Empty;
    }
    public class SoftSkill
    {
        public int Order { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
    }
    public class Section
    {
        public int Order { get; set; } = 0;
        public string Title { get; set; } = string.Empty;
        
        public string ImagePath { get; set; } = string.Empty;

        public ISectionContent? Content { get; set; } = null;

        public string CssClass { get; set; } = string.Empty;
    }
    public interface ISectionContent
    {
    }
    public class EducationCollectionContent : ISectionContent
    {
        public List<EducationContent> Educations { get; set; } = new List<EducationContent>();
    }
    public class EducationContent
    {
        public int Order { get; set; } = 0;
        public int StartYear { get; set; } = 0;
        public int EndYear { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public string Place { get; set; } = string.Empty;
    }
    public class ExperienceCollectionContent : ISectionContent
    {
        public List<ExperienceContent> Experiences { get; set; } = new List<ExperienceContent>();
    }
    public class ExperienceContent
    {
        public int Order { get; set; } = 0;
        public int StartYear { get; set; } = 0;
        public int EndYear { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public string Place { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
    public class TextContent : ISectionContent
    {
        public string Text { get; set; } = string.Empty;
    }
}