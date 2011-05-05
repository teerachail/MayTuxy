using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace TheS.SperfGames.MayaTukky
{
    public class GameService : IGameService
    {
        public void RequestStatistics(string username, Action<PlayerStatistics> callback)
        {
            var result = new PlayerStatistics {
                FirstAveragePoint = 560,
                FirstHighScorePoint = 1023,
                SecondAveragePoint = 233,
                SecondHighScorePoint = 564,
                ThirdAveragePoint = 351,
                ThirdHighScorePoint = 452,
                Percentile = 87.63,
                Time = 72
            };

            callback(result);
        }


        public void RequestScoreTable(Action<ScoreTableResponse> callback)
        {
            // TODO: กำหนดค่า Appo table ตัวอย่าง พี่บุ๊งยังไม่ได้ตัดสินใจ
            var table = new ScoreTableResponse {
                CardLevelList = new List<CardInformation> {
                    new CardInformation{
                        RequireScore = 0,
                        Rank = 1,
                        Name = "Toad",
                        ImageUrl = "ToadCard",
                    },
                    new CardInformation{
                        RequireScore = 100,
                        Rank = 1,
                        Name = "Crow",
                        ImageUrl = "CrowCard"
                    },
                    new CardInformation{
                        RequireScore = 200,
                        Rank = 1,
                        Name = "Ghost",
                        ImageUrl = "GhostCard"
                    },
                    new CardInformation{
                        RequireScore = 300,
                        Rank = 1,
                        Name = "Wolf",
                        ImageUrl = "WolfCard"
                    },
                    new CardInformation{
                        RequireScore = 400,
                        Rank = 2,
                        Name = "Tree",
                        ImageUrl = "TreeCard"
                    },
                    new CardInformation{
                        RequireScore = 500,
                        Rank = 2,
                        Name = "Golem",
                        ImageUrl = "GolemCard"
                    },
                    new CardInformation{
                        RequireScore = 600,
                        Rank = 2,
                        Name = "Gobin",
                        ImageUrl = "GobinCard"
                    },
                    new CardInformation{
                        RequireScore = 700,
                        Rank = 2,
                        Name = "Tribe",
                        ImageUrl = "TribeCard"
                    },
                    new CardInformation{
                        RequireScore = 800,
                        Rank = 3,
                        Name = "Knight",
                        ImageUrl = "KnightCard"
                    },
                    new CardInformation{
                        RequireScore = 900,
                        Rank = 3,
                        Name = "Astrologer",
                        ImageUrl = "AstrologerCard"
                    },
                    new CardInformation{
                        RequireScore = 1000,
                        Rank = 3,
                        Name = "Herbalists",
                        ImageUrl = "HerbalistsCard"
                    },
                    new CardInformation{
                        RequireScore = 1100,
                        Rank = 3,
                        Name = "Priest",
                        ImageUrl = "PriestCard"
                    },
                    new CardInformation{
                        RequireScore = 1200,
                        Rank = 4,
                        Name = "Sorcerer first",
                        ImageUrl = "SorcererFirstCard"
                    },
                    new CardInformation{
                        RequireScore = 1300,
                        Rank = 4,
                        Name = "Sorcerer second",
                        ImageUrl = "SorcererSecondCard"
                    },
                    new CardInformation{
                        RequireScore = 1400,
                        Rank = 4,
                        Name = "Sorcerer third",
                        ImageUrl = "SorcererThirdCard"
                    },
                    new CardInformation{
                        RequireScore = 1500,
                        Rank = 4,
                        Name = "Berserk",
                        ImageUrl = "BerserkCard"
                    },
                    new CardInformation{
                        RequireScore = 1600,
                        Rank = 5,
                        Name = "Heimdell",
                        ImageUrl = "HeimdellCard"
                    },
                    new CardInformation{
                        RequireScore = 1700,
                        Rank = 5,
                        Name = "Thor",
                        ImageUrl = "ThorCard"
                    },
                    new CardInformation{
                        RequireScore = 1800,
                        Rank = 5,
                        Name = "Odin",
                        ImageUrl = "OdinCard"
                    },
                },
            };

            callback(table);
        }
    }
}
