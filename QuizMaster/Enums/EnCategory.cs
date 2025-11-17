using System.ComponentModel;

namespace QuizMaster.Enums
{
    public enum EnCategory
    {
        [Description("General Knowledge")]
        GeneralKnowledge = 9,
        [Description("Entertainment: Books")]
        EntertainmentBooks = 10,
        [Description("Entertainment: Film")]
        EntertainmentFilm = 11,
        [Description("Entertainment: Music")]
        EntertainmentMusic = 12,
        [Description("Entertainment: Musicals & Theatres")]
        EntertainmentMusicalsAndTheatres = 13,
        [Description("Entertainment: Television")]
        EntertainmentTelevision = 14,
        [Description("Entertainment: Video Games")]
        EntertainmentVideoGames = 15,
        [Description("Entertainment: Board Games")]
        EntertainmentBoardGames = 16,
        [Description("Science & Nature")]
        ScienceAndNature = 17,
        [Description("Science: Computers")]
        ScienceComputers = 18,
        [Description("Science: Mathematics")]
        ScienceMathematics = 19,
        [Description("Mythology")]
        Mythology = 20,
        [Description("Sports")]
        Sports = 21,
        [Description("Geography")]
        Geography = 22,
        [Description("History")]
        History = 23,
        [Description("Politics")]
        Politics = 24,
        [Description("Art")]
        Art = 25,
        [Description("Celebrities")]
        Celebrities = 26,
        [Description("Animals")]
        Animals = 27,
        [Description("Vehicles")]
        Vehicles = 28,
        [Description("Entertainment: Comics")]
        EntertainmentComics = 29,
        [Description("Science: Gadgets")]
        ScienceGadgets = 30,
        [Description("Entertainment: Japanese Anime & Manga")]
        EntertainmentJapaneseAnimeAndManga = 31,
        [Description("Entertainment: Cartoon & Animations")]
        EntertainmentCartoonAndAnimations = 32
    }
}
