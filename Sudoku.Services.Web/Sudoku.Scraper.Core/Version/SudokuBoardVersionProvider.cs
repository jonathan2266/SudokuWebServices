using HtmlAgilityPack;
using Sudoku.Parser.Readers;

namespace Sudoku.Scraper.Core.Version
{
    public class SudokuBoardVersionProvider : IProvideBoardNumber
    {
        public async Task<BoardNumber> Read(IReader reader)
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
