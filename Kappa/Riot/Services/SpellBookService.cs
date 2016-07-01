using Kappa.Riot.Domain;
using System.Threading.Tasks;
using Kappa.BackEnd;

namespace Kappa.Riot.Services {
  internal class SpellBookService : Service {
    protected override string Destination => "spellBookService";

    public SpellBookService(Session session) : base(session) { }

    /// <summary>
    /// Gets the runes for a user
    /// </summary>
    /// <param name="summonerId">The summoner ID for the user</param>
    /// <returns>Returns the rune pages for a user</returns>
    public Task<object> GetSpellBook(long summonerId) {
      return InvokeAsync<object>("getSpellBook", summonerId);
    }

    /// <summary>
    /// Selects a rune page for use
    /// </summary>
    /// <param name="spellbookPage">The spellbook page the player wants to use</param>
    /// <returns>The selected spellbook page</returns>
    public Task<object> SelectDefaultSpellBookPage(SpellBookPageDTO spellbookPage) {
      return InvokeAsync<object>("selectDefaultSpellBookPage", spellbookPage);
    }

    /// <summary>
    /// Saves the players spellbook
    /// </summary>
    /// <param name="spellbook">The players SpellBookDTO</param>
    public Task<object> SaveSpellBook(SpellBookDTO spellbook) {
      return InvokeAsync<object>("saveSpellBook", spellbook);
    }
  }
}
