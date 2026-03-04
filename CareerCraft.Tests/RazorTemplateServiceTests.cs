using CareerCraft.Core.Services;
using CareerCraft.Infrastructure.Services;
using CareerCraft.Core.ViewModels;

namespace CareerCraft.Tests;

public class RazorTemplateServiceTests
{
    private readonly ITemplateService? _templateService = null;
    public static TemplateViewModel GetSampleTemplateViewModel()
    {
        var viewModel = new TemplateViewModel
        {
            FirstName = "FirstName",
            LastName = "LastName",
            JobTitle = "Job title here and this can be long",
            Infos = [
                new()
                {
                    Order = 1,
                    ImagePath = "Resources/phone.svg",
                    CustomCssClass = "wt",
                    Value = "+1 555 5555"
                },
                new()
                {
                    Order = 2,
                    ImagePath = "Resources/mail.svg",
                    CustomCssClass = "wt",
                    Value = "some.email@some-provider.com"
                },
                new()
                {
                    Order = 3,
                    ImagePath = "Resources/location.svg",
                    CustomCssClass = "wt",
                    Value = "My cool street, 100, 20000"
                },
                new()
                {
                    Order = 4,
                    ImagePath = "Resources/linkedIn.webp",
                    //CustomCssClass = "wt",
                    Value = "linkedin.com/in/short-tag"
                },
                new()
                {
                    Order = 5,
                    ImagePath = "Resources/calendar.svg",
                    CustomCssClass = "wt",
                    Value = "Né le 01/01/2000"
                },
                new()
                {
                    Order = 6,
                    ImagePath = "Resources/car.svg",
                    CustomCssClass = "wt",
                    Value = "Permis B"
                },
            ],
            SoftSkills = [
                new() { Order = 1, Name = "soft skill 1"},
                new() { Order = 2, Name = "soft skill 2"},
                new() { Order = 3, Name = "soft skill 3"},
                new() { Order = 4, Name = "soft skill 4"},
                new() { Order = 5, Name = "soft skill 5"},
            ],
            Sections = [
                new() { Order = 1, CssClass = "section-pill", Title = "Objectifs", ImagePath = "Resources/target.svg"
                    , Content = new TemplateViewModel.TextContent {
                        Text = "job description here. <b>BouliBoula</b> is the best!"
                    } },
                new() { Order = 1, CssClass = "section-pill", Title = "Compétences technique", ImagePath = "Resources/tools.svg"
                    , Content = new TemplateViewModel.TextContent {
                        Text = @"
                        <ul>
                            <li><span class='skill-cat'>Réseaux:</span> Cisco (académique), VLAN, TCP/IP, NAT, DNS, DHCP, Wireshark, interconnexion sites</li>
                            <li><span class='skill-cat'>Téléphonie VoIP:</span> Trixbox, trunks SIP, routage inter-sites, redondance</li>
                            <li><span class='skill-cat'>Systèmes & Infrastructure:</span> Active Directory, Windows Server, Linux, Firewall Windows</li>
                            <li><span class='skill-cat'>Sécurité:</span> Gestion accès, segmentation, mises à jour hors heures</li>
                        </ul>"
                    } },
                new() { Order = 2, CssClass = "section-pill", Title = "Expériences et Projets", ImagePath = "Resources/experiences.webp"
                    , Content = new TemplateViewModel.ExperienceCollectionContent {
                        Experiences =
                        [
                            new() { Order = 1, Name = "Experience 1", Place = "Place 1", StartYear = 2020, EndYear = 2021 },
                            new() { Order = 2, Name = "Experience 2", Place = "Place 2", StartYear = 2021, EndYear = 2022 },
                            new() { Order = 3, Name = "Experience 3 formidable", Place = "Place 3", StartYear = 2022, EndYear = 2023 },
                        ]
                    } },
                new() { Order = 3, CssClass = "section-pill", Title = "Formations", ImagePath = "Resources/academy.webp"
                    , Content = new TemplateViewModel.EducationCollectionContent {
                        Educations =
                        [
                            new() { Order = 1, Name = "Education 1", Place = "Place 1", StartYear = 2020, EndYear = 2021 },
                            new() { Order = 2, Name = "Education 2", Place = "Place 2", StartYear = 2021, EndYear = 2022 },
                            new() { Order = 3, Name = "Education 3", Place = "Place 3", StartYear = 2022, EndYear = 0 },
                        ]
                    } 
                },
            ]
        };

        return viewModel;
    }
    
    public RazorTemplateServiceTests()
    {
        _templateService = new RazorTemplateService();
    }

    [Fact]
    public async Task CanGenerateTemplateFromViewModel()
    {        
        var viewModel = GetSampleTemplateViewModel();
        string result = await _templateService.RenderAsync("normal", viewModel);

        Assert.Contains("BouliBoula", result);
        Assert.Contains("Education 3", result);
        Assert.Contains("Experience 3 formidable", result);
        Assert.NotEmpty(result);
    }
}