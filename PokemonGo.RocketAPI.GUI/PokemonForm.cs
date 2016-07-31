using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PokemonGo.RocketAPI.Enums;
using System.Net;
using System.IO;
using PokemonGo.RocketAPI.GeneratedCode;
using System.Collections;
using System.Diagnostics;

namespace PokemonGo.RocketAPI.GUI
{
    public partial class PokemonForm : Form
    {
        Client client;

        public PokemonForm(Client client)
        {
            this.client = client;
            InitializeComponent();
        }

        private async void PokemonForm_Load(object sender, EventArgs e)
        {
            await Execute();
        }

        private async Task Execute()
        {            
            pokemonListView.Clear();
            var inventory = await client.GetInventory();

            var pokemons =
                inventory.InventoryDelta.InventoryItems
                .Select(i => i.InventoryItemData?.Pokemon)
                    .Where(p => p != null && p?.PokemonId > 0)
                    .OrderByDescending(key => key.Cp);

            var families = inventory.InventoryDelta.InventoryItems
                .Select(i => i.InventoryItemData?.PokemonFamily)
                .Where(p => p != null && (int)p?.FamilyId > 0)
                .OrderByDescending(p => (int)p.FamilyId);

            var imageList = new ImageList { ImageSize = new Size(50, 50) };
            pokemonListView.ShowItemToolTips = true;

            // Add Pokemon ListViewItems to a list
            var pokeList = new List<ListViewItem>();
            foreach (var pokemon in pokemons)
            {
                ListViewItem listViewItem = new ListViewItem();

                // Get Pokemon Image Index
                var imageIndex = imageList.Images.IndexOfKey(pokemon.PokemonId.ToString());

                // Checker for Pokemon Image
                if (imageIndex == -1)
                    imageList.Images.Add(pokemon.PokemonId.ToString(), await GetPokemonImageAsync((int)pokemon.PokemonId));
                else
                    imageList.Images.Add(pokemon.PokemonId.ToString(), imageList.Images[imageIndex]);

                pokemonListView.LargeImageList = imageList;
                var pokemonIv = Math.Floor(Logic.Logic.CalculatePokemonPerfection(pokemon));
                listViewItem.SubItems.Add(pokemon.PokemonId.ToString());
                listViewItem.SubItems.Add(pokemon.Cp.ToString());
                listViewItem.SubItems.Add(pokemonIv.ToString());

                var currentCandy = families
                    .Where(i => (int)i.FamilyId <= (int)pokemon.PokemonId)
                    .Select(f => f.Candy)
                    .First();

                listViewItem.ImageKey = pokemon.PokemonId.ToString();
                listViewItem.Text = string.Format("{0}\nCP {1} IV {2}%", pokemon.PokemonId, pokemon.Cp, pokemonIv);
                listViewItem.Tag = pokemon.Id;
                listViewItem.ToolTipText = "Candy: " + currentCandy;

                pokeList.Add(listViewItem);
            }

            // Add all Pokemon ListViewItems to pokemonListView all at once
            ListViewItem[] pokeArray = pokeList.ToArray();
            pokemonListView.BeginUpdate();
            pokemonListView.Items.AddRange(pokeArray);
            pokemonListView.EndUpdate();
        }

        private async Task<Bitmap> GetPokemonImageAsync(int pokemonId)
        {
            WebRequest req = WebRequest.Create("http://pokeapi.co/media/sprites/pokemon/"+pokemonId+".png");
            WebResponse res = await req.GetResponseAsync();
            Stream resStream = res.GetResponseStream();
            return new Bitmap(resStream);
        }

        private void pokemonListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            MessageBox.Show(pokemonListView.SelectedItems[0].Tag.ToString());
        }

        private async void btnTransfer_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Do you really want to transfer selected pokemon(s)?\nYou will never see them again :(", "Confirm Transfer", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                foreach (ListViewItem item in pokemonListView.SelectedItems)
                {
                    var id = (ulong)item.Tag;
                    await client.TransferPokemon(id);
                }
            }

            await Execute();      
        }

        private void pokemonListViewItemSorter(int subItemsColumn)
        {
            ItemComparer sorter = pokemonListView.ListViewItemSorter as ItemComparer;

            if (sorter == null)
            {
                sorter = new ItemComparer(subItemsColumn);

                // Bug fix for IV (Occurs when Sort IV is selected first, as it should Descend)
                if (subItemsColumn == 3)
                    sorter.Order = SortOrder.Ascending;

                pokemonListView.ListViewItemSorter = sorter;
            }

            // If clicked column is already the column that is being sorted
            if (subItemsColumn == sorter.Column)
            {
                // Reverse the current sort direction
                if (sorter.Order == SortOrder.Ascending)
                    sorter.Order = SortOrder.Descending;
                else
                    sorter.Order = SortOrder.Ascending;
            }
            else
            {
                // Set the column number that is to be sorted.
                sorter.Column = subItemsColumn;

                // Default Sort Order for Cp
                if (subItemsColumn == 2)
                    sorter.Order = SortOrder.Descending;

                // Default Sort Order for IV
                else if (subItemsColumn == 3)
                    sorter.Order = SortOrder.Descending;

                // Default Sort Order for Name
                else if (subItemsColumn == 1)
                    sorter.Order = SortOrder.Ascending;
            }

            pokemonListView.Sort();
        }

        private void sortByCPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pokemonListViewItemSorter(2);
        }

        private void sortByIVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pokemonListViewItemSorter(3);
        }

        private void sortByNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pokemonListViewItemSorter(1);
        }
    }

    public class ItemComparer : IComparer
    {
        // Column used for comparison
        public int Column { get; set; }

        // Order of sorting
        public SortOrder Order { get; set; }

        public ItemComparer(int colIndex)
        {
            Column = colIndex;
            Order = SortOrder.None;
        }

        public int Compare(object a, object b)
        {
            int result;

            ListViewItem itemA = a as ListViewItem;
            ListViewItem itemB = b as ListViewItem;

            if (itemA == null && itemB == null)
                result = 0;
            else if (itemA == null)
                result = -1;
            else if (itemB == null)
                result = 1;
            if (itemA == itemB)
                result = 0;

            /*
            // datetime comparison
            DateTime x1, y1;
            // Parse the two objects passed as a parameter as a DateTime.
            if (!DateTime.TryParse(itemA.SubItems[Column].Text, out x1))
                x1 = DateTime.MinValue;
            if (!DateTime.TryParse(itemB.SubItems[Column].Text, out y1))
                y1 = DateTime.MinValue;
            result = DateTime.Compare(x1, y1);
            if (x1 != DateTime.MinValue && y1 != DateTime.MinValue)
                goto done;
            */

            // Numeric comparison
            decimal x2, y2;

            if (!Decimal.TryParse(itemA.SubItems[Column].Text, out x2))
                x2 = Decimal.MinValue;
            if (!Decimal.TryParse(itemB.SubItems[Column].Text, out y2))
                y2 = Decimal.MinValue;
            result = Decimal.Compare(x2, y2);
            if (x2 != Decimal.MinValue && y2 != Decimal.MinValue)
                goto done;

            //alphabetic comparison
            result = String.Compare(itemA.SubItems[Column].Text, itemB.SubItems[Column].Text);

        done:

            // If sort order is descending.
            if (Order == SortOrder.Descending)
                // Invert the value returned by Compare.
                result *= -1;
            return result;
        }
    }
}
