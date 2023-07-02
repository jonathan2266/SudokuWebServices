using HtmlAgilityPack;
using Sudoku.Parser.Readers;
using Sudoku.Scraper.Core.Configuration;
using Sudoku.Scraper.Domain.Entities;
using System.Diagnostics;

namespace Sudoku.Scraper.Core.Services.Version
{
    public class SudokuBoardVersionProvider : IProvideBoardNumber
    {
        private readonly static ActivitySource _activitySource = new(ActivityKeys.Core);

        public async Task<BoardNumber> Read(IReader reader)
        {
            using (var source = _activitySource.StartActivity("Puzzle.ReadVersion"))
            {
                var stream = await reader.GetStream();
                
                var document = new HtmlDocument();
                document.Load(stream, reader.StreamEncoding);

                var infoElement = document.GetElementbyId("infos");

                var matchingNode = infoElement.SelectNodes("//*[contains(., 'n°')]");
                var spanNodeWithNumber = matchingNode.First(x => x.Name == "span");
                var rawBoardNumber = spanNodeWithNumber.InnerText.Split("n°").Last().Trim();

                return new BoardNumber(rawBoardNumber);
            }
        }
    }
}
