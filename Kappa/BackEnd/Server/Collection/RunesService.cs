using Kappa.Riot.Domain;
using System;
using System.Linq;

namespace Kappa.BackEnd.Server.Collection {
    [Docs("group", "Collection")]
    public class RunesService : JSONService {
        private SpellBookDTO book;
        private Session session;

        public RunesService(Session session) : base("/collection/runes") {
            this.session = session;
            session.Authed += Kappa_Authed;
        }

        private void Kappa_Authed(object sender, EventArgs e) {
            book = session.Me.Runes;
        }

        [Endpoint("/get")]
        public SpellBookDTO Get() {
            return book;
        }

        [Endpoint("/save")]
        public async void Save(SpellBookPageDTO page) {
            var edited = book.BookPages.Single(p => p.PageId == page.PageId);
            book.BookPages.ForEach(p => p.Current = false);
            book.BookPages.Remove(edited);
            book.BookPages.Add(page);
            page.Current = true;

            await this.session.SpellBookService.SaveSpellBook(book);
        }

        [Endpoint("/select")]
        public async void Select(long pageId) {
            var selected = book.BookPages.Single(p => p.PageId == pageId);
            book.BookPages.ForEach(p => p.Current = false);
            selected.Current = true;

            await this.session.SpellBookService.SelectDefaultSpellBookPage(selected);
        }
    }
}
