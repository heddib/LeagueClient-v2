using Kappa.Riot.Domain;
using System;
using System.Diagnostics;
using System.Linq;
using Kappa.BackEnd.Server.Collection.Model;

namespace Kappa.BackEnd.Server.Collection {
    [Docs("group", "Collection")]
    public class RunesService : JSONService {
        private SpellBookDTO srcBook;
        private RuneBook book;
        private Session session;

        public RunesService(Session session) : base("/collection/runes") {
            this.session = session;
            session.Authed += Kappa_Authed;
        }

        private void Kappa_Authed(object sender, EventArgs e) {
            srcBook = this.session.Me.Runes;

            book = new RuneBook();
            foreach (var srcPage in srcBook.BookPages) {
                var page = new RunePage {
                    Id = srcPage.PageId,
                    Name = srcPage.Name
                };
                if (srcPage.Current) book.Selected = page.Id;

                foreach (var entry in srcPage.SlotEntries) {
                    page.Runes.Add(entry.RuneSlotId.ToString(), entry.RuneId);
                }

                book.Pages.Add(page);
            }
        }

        [Endpoint("/get")]
        public RuneBook Get() {
            return book;
        }

        [Endpoint("/save")]
        public async void Save(RunePage page) {
            var old = book.Pages.Single(p => p.Id == page.Id);
            book.Pages.Remove(old);
            book.Pages.Add(page);

            book.Selected = page.Id;

            var edited = srcBook.BookPages.Single(p => p.PageId == page.Id);
            srcBook.BookPages.ForEach(p => p.Current = false);
            edited.Current = true;
            edited.SlotEntries.Clear();

            foreach (var rune in page.Runes) {
                edited.SlotEntries.Add(new SlotEntry {
                    RuneSlotId = int.Parse(rune.Key),
                    RuneId = rune.Value
                });
            }

            await this.session.SpellBookService.SaveSpellBook(srcBook);
        }

        [Endpoint("/select")]
        public async void Select(long pageId) {
            book.Selected = pageId;

            var selected = srcBook.BookPages.Single(p => p.PageId == pageId);
            srcBook.BookPages.ForEach(p => p.Current = false);
            selected.Current = true;

            try {
                await this.session.SpellBookService.SaveSpellBook(srcBook);
            } catch (Exception x) {
                Debug.WriteLine("");
                Debug.WriteLine("Rune page editing failed!!!!!!");
                Debug.WriteLine("");
                Debug.WriteLine(x);
                await this.session.SpellBookService.SelectDefaultSpellBookPage(selected);
            }
        }
    }
}
