using CareerCraft.Core.Services;
using CareerCraft.Infrastructure.Services;
using CareerCraft.Core.ViewModels;

namespace CareerCraft.Tests;

public class RazorTemplateServiceTests
{
    [Fact]
    public async Task CanGenerateTemplateFromViewModel()
    {
        ITemplateService templateService = new RazorTemplateService();

        var viewModel = new TemplateViewModel()
        {
            FirstName = "FirstName",
            LastName = "LastName",
            JobTitle = "Job title here"
        };
        viewModel.SoftSkills = new List<TemplateViewModel.SoftSkill>()
        {
            new TemplateViewModel.SoftSkill { Order = 1, Name = "soft skill 1"},
            new TemplateViewModel.SoftSkill { Order = 2, Name = "soft skill 2"},
            new TemplateViewModel.SoftSkill { Order = 3, Name = "soft skill 3"},
            new TemplateViewModel.SoftSkill { Order = 4, Name = "soft skill 4"},
            new TemplateViewModel.SoftSkill { Order = 5, Name = "soft skill 5"},
        };
        viewModel.Sections = new List<TemplateViewModel.Section>()
        {
            new TemplateViewModel.Section { Order = 1, CssClass = "section-pill", Title = "Objectifs", ImagePath = "Resources/target.svg"
                , Content = new TemplateViewModel.TextContent {
                    Text = "job description here. <b>BouliBoula</b> is the best!"
                } },
            new TemplateViewModel.Section { Order = 1, CssClass = "section-pill", Title = "Compétences technique", ImagePath = "Resources/tools.svg"
                , Content = new TemplateViewModel.TextContent {
                    Text = @"<ul>
                        <li><span class='skill-cat'>Réseaux:</span> Cisco (académique), VLAN, TCP/IP, NAT, DNS, DHCP, Wireshark, interconnexion sites</li>
                        <li><span class='skill-cat'>Téléphonie VoIP:</span> Trixbox, trunks SIP, routage inter-sites, redondance</li>
                        <li><span class='skill-cat'>Systèmes & Infrastructure:</span> Active Directory, Windows Server, Linux, Firewall Windows</li>
                        <li><span class='skill-cat'>Sécurité:</span> Gestion accès, segmentation, mises à jour hors heures</li>
                    </ul>"
                } },
            new TemplateViewModel.Section { Order = 2, CssClass = "section-pill", Title = "Expériences et Projets", ImagePath = "Resources/experiences.webp"
                , Content = new TemplateViewModel.ExperienceCollectionContent {
                    Experiences = new List<TemplateViewModel.ExperienceContent>()
                    {
                        new TemplateViewModel.ExperienceContent { Order = 1, Name = "Experience 1", Place = "Place 1", StartYear = 2020, EndYear = 2021 },
                        new TemplateViewModel.ExperienceContent { Order = 2, Name = "Experience 2", Place = "Place 2", StartYear = 2021, EndYear = 2022 },
                        new TemplateViewModel.ExperienceContent { Order = 3, Name = "Experience 3 formidable", Place = "Place 3", StartYear = 2022, EndYear = 2023 },
                    }
                } },
            new TemplateViewModel.Section { Order = 3, CssClass = "section-pill", Title = "Formations", ImagePath = "Resources/academy.webp"
                , Content = new TemplateViewModel.EducationCollectionContent {
                    Educations = new List<TemplateViewModel.EducationContent>()
                    {
                        new TemplateViewModel.EducationContent { Order = 1, Name = "Education 1", Place = "Place 1", StartYear = 2020, EndYear = 2021 },
                        new TemplateViewModel.EducationContent { Order = 2, Name = "Education 2", Place = "Place 2", StartYear = 2021, EndYear = 2022 },
                        new TemplateViewModel.EducationContent { Order = 3, Name = "Education 3", Place = "Place 3", StartYear = 2022, EndYear = 0 },
                    }
                } },
        };
        string result = await templateService.RenderAsync("normal", viewModel);

        Assert.Contains("BouliBoula", result);
        Assert.Contains("Education 3", result);
        Assert.Contains("Experience 3 formidable", result);
        Assert.NotEmpty(result);
    }
}