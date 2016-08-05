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
using static PokemonGo.RocketAPI.GeneratedCode.EvolvePokemonOut.Types;
using PokemonGo.RocketAPI.Logic;
using PokemonGo.RocketAPI.GUI.Helpers;
using PokemonGo.RocketAPI.GUI.Extensions;
using System.Runtime.InteropServices;

namespace PokemonGo.RocketAPI.GUI
{
    public partial class PokemonForm : Form
    {
        private Client _client;
        private Inventory _inventory;
        private ItemComparer _sorter;
        private List<ListViewItem> _pokemonListViewBackup;

        public PokemonForm(Client client)
        {
            _client = client;
            _inventory = new Inventory(client);
            _pokemonListViewBackup = new List<ListViewItem>();
            InitializeComponent();

            // Define _sorter
            _sorter = pokemonListView.ListViewItemSorter as ItemComparer;

            // Set Default Column to CP
            _sorter = new ItemComparer(2);

            // Set Default Sort
            _sorter.Order = SortOrder.Descending;

            pokemonListView.ListViewItemSorter = _sorter;

            // Set Search Textbox Watermark
            searchTextBox.SetWatermark("Search Pokemon...");
        }

        private async void PokemonForm_Load(object sender, EventArgs e)
        {
            try
            {
                await Execute();
            }
            catch (Exception ex)
            {
                ErrorReportCreator.Create("MyPokemonList", "Unable to Open My Pokemon List", ex);
            }           
        
        }

        private async Task Execute()
        {
            disableButtonsDuringLoading();
            resetLoadProgress();
            pokemonListView.Clear();
            pokemonListView.ShowItemToolTips = true;

            // Get Pokemons and Pokemon Families
            var pokemons = await _inventory.GetPokeListPokemon();
            var families = await _inventory.GetPokeListPokemonFamilies();

            var imageList = new ImageList { ImageSize = new Size(50, 50) };
            var myPokemonsCount = pokemons.Count();

            // Add Pokemon ListViewItems to a list
            var pokeList = new List<ListViewItem>();
            var pokeIndex = 0;
            foreach (var pokemon in pokemons)
            {
                ListViewItem listViewItem = new ListViewItem();

                // Get Pokemon Index Id
                var pokemonIndexId = (int)pokemon.PokemonId;

                // Get Pokemon Family Id
                var pokemonFamilyId = families
                    .Where(i => (int)i.FamilyId <= pokemonIndexId)
                    .Select(f => f.FamilyId)
                    .First();

                // Get Pokemon Capture Date / Time
                DateTime pokemonCaptureDate = GetPokemonCaptureDate(pokemon.CreationTimeMs);

                // Get Pokemon Image File Name
                var pokemonFileName = pokemonIndexId + ".png";

                // Get Image Folder Name
                var imageFolderName = "Images";

                // Check if Pokemon Image Exists in Directory
                if (File.Exists(imageFolderName + "\\" + pokemonFileName))
                {
                    Image myPokemonImage = Image.FromFile(imageFolderName + "\\" + pokemonFileName);
                    imageList.Images.Add(pokemon.PokemonId.ToString(), myPokemonImage);
                }
                else
                {
                    // Checker for Image Directory
                    if (!Directory.Exists(imageFolderName))
                        Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), imageFolderName));

                    imageList.Images.Add(pokemon.PokemonId.ToString(), await GetPokemonImageAsync(pokemonFileName));
                }

                pokemonListView.LargeImageList = imageList;

                // Get Pokemon IV
                var pokemonIv = Math.Floor(Logic.Logic.CalculatePokemonPerfection(pokemon));

                // Add Extra Pokemon Info to Item
                listViewItem.SubItems.Add(pokemon.PokemonId.ToString()); // Col 1: Name
                listViewItem.SubItems.Add(pokemon.Cp.ToString()); // Col 2: CP
                listViewItem.SubItems.Add(pokemonIv.ToString()); // Col 3: IV
                listViewItem.SubItems.Add(pokemonIndexId.ToString()); // Col 4: Index Number
                listViewItem.SubItems.Add(pokemonFamilyId.ToString()); // Col 5: Family Id
                listViewItem.SubItems.Add(pokemonCaptureDate.ToString()); // Col 6: Capture Date

                // Get Pokemon Candies
                var currentCandy = families
                    .Where(i => (int)i.FamilyId <= pokemonIndexId)
                    .Select(f => f.Candy)
                    .First();

                // Set Pokemon Item Info
                listViewItem.ImageKey = pokemon.PokemonId.ToString();
                listViewItem.Text = string.Format("{0}\nCP {1} IV {2}%", pokemon.PokemonId, pokemon.Cp, pokemonIv);
                listViewItem.Tag = pokemon.Id;
                listViewItem.ToolTipText = "Candy: " + currentCandy;

                // Pokemon List Loading Progress
                pokeIndex += 1;
                lbPokeListLoading.Text = $"{pokeIndex}/{myPokemonsCount}";

                pokeList.Add(listViewItem);
            }

            lbPokeListLoading.Visible = false;

            // Add all Pokemon ListViewItems to pokemonListView all at once
            ListViewItem[] pokeArray = pokeList.ToArray();
            pokemonListView.BeginUpdate();
            pokemonListView.Items.AddRange(pokeArray);
            pokemonListView.EndUpdate();

            // Search function requires a pokemonListView backup
            _pokemonListViewBackup.Clear();
            foreach (ListViewItem item in pokemonListView.Items)
                _pokemonListViewBackup.Add(item);

            pokemonListView.Sort();
            enableButtonsAfterLoading();
        }

        private static DateTime GetPokemonCaptureDate(ulong milliseconds)
        {
            DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return start.AddMilliseconds(milliseconds).ToLocalTime();
        }

        private void enableButtonsAfterLoading()
        {
            // Enable Buttons
            sortToolStripMenuItem.Enabled = true;
            actionsToolStripMenuItem.Enabled = true;
            searchTextBox.Enabled = true;
        }

        private void disableButtonsDuringLoading()
        {
            // Disable Buttons
            sortToolStripMenuItem.Enabled = false;
            actionsToolStripMenuItem.Enabled = false;
            searchTextBox.Enabled = false;
        }

        private void resetLoadProgress()
        {
            lbPokeListLoading.Text = "Loading...";
            lbPokeListLoading.Visible = true;
        }

        private async Task<Image> GetPokemonImageAsync(string pokemonFileName)
        {
            string localFileName = "Images\\" + pokemonFileName;
            WebRequest request = WebRequest.Create("http://pokeapi.co/media/sprites/pokemon/" + pokemonFileName);
            WebResponse response = await request.GetResponseAsync();
            Image pokemonImage = Image.FromStream(response.GetResponseStream());
            pokemonImage.Save(localFileName);
            return pokemonImage;
        }

        private void pokemonListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            MessageBox.Show(pokemonListView.SelectedItems[0].Tag.ToString());
        }

        private void pokemonListViewItemSorter(int subItemsColumn)
        {
            // Deselect and remove focus on ListView items before sort.
            // We're not using pokemonListView.SelectedItems to prevent
            // sort fixated user view bugs that pop-up from time to time
            foreach (ListViewItem item in pokemonListView.Items)
            {
                item.Selected = false;
                item.Focused = false;
            }

            // If clicked column is already the column that is being sorted
            if (subItemsColumn == _sorter.Column)
            {
                // Reverse the current sort direction
                if (_sorter.Order == SortOrder.Ascending)
                    _sorter.Order = SortOrder.Descending;
                else
                    _sorter.Order = SortOrder.Ascending;
            }
            else
            {
                // Set the column number that is to be sorted.
                _sorter.Column = subItemsColumn;

                // Default Sort Order for Cp
                if (subItemsColumn == 2)
                    _sorter.Order = SortOrder.Descending;

                // Default Sort Order for IV
                else if (subItemsColumn == 3)
                    _sorter.Order = SortOrder.Descending;

                // Default Sort Order for Name
                else if (subItemsColumn == 1)
                    _sorter.Order = SortOrder.Ascending;

                // Default Sort Order for Newest
                else if (subItemsColumn == 6)
                    _sorter.Order = SortOrder.Descending;

                // Default Sort Order for Index Number
                else if (subItemsColumn == 4)
                    _sorter.Order = SortOrder.Ascending;
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

        private void sortByNewestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pokemonListViewItemSorter(6);
        }

        private void sortByIndexNumberToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pokemonListViewItemSorter(4);
        }

        private async Task refreshPokemonCandyTips(List<string> uniquePokemonList)
        {
            // Checker for Unique Evolved Pokemon Families
            if (uniquePokemonList.Count() != 0)
            {
                // Get Pokemon Families
                var families = await _inventory.GetPokeListPokemonFamilies();

                foreach (ListViewItem item in pokemonListView.Items)
                {
                    // Get Pokemon Family Id
                    var pokemonFamilyId = item.SubItems[5].Text;

                    // Get Pokemon Index Id
                    var pokemonIndexId = int.Parse(item.SubItems[4].Text);

                    // Get Pokemon Candies
                    var currentCandy = families
                        .Where(i => (int)i.FamilyId <= pokemonIndexId)
                        .Select(f => f.Candy)
                        .First();

                    // Updates Candies of Pokemon related to any Evolved Pokemon Family
                    if (uniquePokemonList.Contains(pokemonFamilyId))
                        item.ToolTipText = "Candy: " + currentCandy;
                }
            }
        }

        private async void transferSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Do you really want to transfer selected pokemon(s)?\nYou will never see them again :(", "Confirm Transfer", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                // Unique Pokemons Dictionary
                List<string> uniquePokemonList = new List<string>();

                // Get Pokemon Families
                var families = await _inventory.GetPokeListPokemonFamilies();

                foreach (ListViewItem item in pokemonListView.SelectedItems)
                {
                    await _client.TransferPokemon((ulong)item.Tag);

                    // Get Pokemon Index Id
                    var pokemonIndexId = int.Parse(item.SubItems[4].Text);

                    // Get Pokemon Family Id
                    var pokemonFamilyId = families
                        .Where(i => (int)i.FamilyId <= pokemonIndexId)
                        .Select(f => f.FamilyId)
                        .First();

                    // Add Unique Pokemon Family Ids
                    if (!uniquePokemonList.Contains(pokemonFamilyId.ToString()))
                        uniquePokemonList.Add(pokemonFamilyId.ToString());

                    // Logger
                    Logger.Write($"Transferred {item.SubItems[1].Text} with {item.SubItems[2].Text} CP and an IV of {item.SubItems[3].Text}.");

                    // Reset Selection / Focus
                    item.Selected = false;
                    item.Focused = false;

                    // Remove items from both lists
                    pokemonListView.Items.Remove(item);
                    _pokemonListViewBackup.Remove(item);
                }

                // Update Pokemon Candy Tips
                await refreshPokemonCandyTips(uniquePokemonList);

                // Logging
                Logger.Write("Finished Transfering Pokemons.");
            }
        }

        private async void evolveSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Do you really want to evolve selected pokemon(s)?", "Confirm evolve", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                foreach (ListViewItem item in pokemonListView.SelectedItems)
                {
                    var newPokemon = await _client.EvolvePokemon((ulong)item.Tag);

                    if (newPokemon.Result == EvolvePokemonStatus.PokemonEvolvedSuccess)
                    {
                        // Logging
                        Logger.Write($"Evolved {item.SubItems[1].Text} to {newPokemon.EvolvedPokemon.PokemonType.ToString()} with {newPokemon.EvolvedPokemon.Cp} CP and an IV of {item.SubItems[3].Text}.");

                        MessageBox.Show($"Congratulations with your new pokemon {newPokemon.EvolvedPokemon.PokemonType.ToString()} with {newPokemon.EvolvedPokemon.Cp} CP!");
                    }
                    else if (newPokemon.Result == EvolvePokemonStatus.FailedInsufficientResources)
                        MessageBox.Show("Insufficient Resources!");
                    else
                        MessageBox.Show($"Error: {newPokemon.Result.ToString()}");
                }

                // Logging
                Logger.Write("Finished Evolving Pokemons.");

                // Reload pokemonListView
                await Execute();
            }
        }

        private async void powerupSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Do you really want to power up selected pokemon(s)?", "Confirm power up", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                // Unique Pokemons Dictionary
                List<string> uniquePokemonList = new List<string>();

                // Get Pokemon Families
                var families = await _inventory.GetPokeListPokemonFamilies();

                foreach (ListViewItem item in pokemonListView.SelectedItems)
                {
                    var poweredUpPokemon = await _client.PowerUpPokemon((ulong)item.Tag);

                    if (poweredUpPokemon.Result == EvolvePokemonStatus.FailedInsufficientResources)
                        MessageBox.Show("Insufficient Resources!");
                    else if (poweredUpPokemon.Result == EvolvePokemonStatus.FailedPokemonCannotEvolve)
                        MessageBox.Show("Unable to powerup more for your current level!");
                    else if (poweredUpPokemon.Result == EvolvePokemonStatus.PokemonEvolvedSuccess)
                    {
                        // Get Pokemon Index Id
                        var pokemonIndexId = int.Parse(item.SubItems[4].Text);

                        // Get Pokemon Family Id
                        var pokemonFamilyId = families
                            .Where(i => (int)i.FamilyId <= pokemonIndexId)
                            .Select(f => f.FamilyId)
                            .First();

                        // Update Pokemon Info
                        item.SubItems[2].Text = poweredUpPokemon.EvolvedPokemon.Cp.ToString();
                        item.Text = string.Format("{0}\nCP {1} IV {2}%", item.SubItems[1].Text, item.SubItems[2].Text, item.SubItems[3].Text);

                        // Add Unique Pokemon Family Ids
                        if (!uniquePokemonList.Contains(pokemonFamilyId.ToString()))
                            uniquePokemonList.Add(pokemonFamilyId.ToString());

                        // Logging
                        Logger.Write($"Powered Up {item.SubItems[1].Text} to {item.SubItems[2].Text} CP with an IV of {item.SubItems[3].Text}.");
                    }
                    else
                        MessageBox.Show($"Error: {poweredUpPokemon.Result.ToString()}");
                }

                // Sort
                pokemonListView.Sort();

                // Update Pokemon Candy Tips
                await refreshPokemonCandyTips(uniquePokemonList);

                // Logging
                Logger.Write("Finished Powering Up Pokemons.");
            }
        }

        private void pokemonListView_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (pokemonListView.FocusedItem.Bounds.Contains(e.Location) == true)
                {
                    ContextMenuStrip myContextMenuStrip = new ContextMenuStrip();
                    myContextMenuStrip.Items.Add("Transfer selected pokémons", null, transferSelectedToolStripMenuItem_Click);
                    myContextMenuStrip.Items.Add("Evolve selected pokémons", null, evolveSelectedToolStripMenuItem_Click);
                    myContextMenuStrip.Items.Add("Powerup selected pokémons", null, powerupSelectedToolStripMenuItem_Click);
                    myContextMenuStrip.Show(pokemonListView, e.Location);
                }
            }
        }

        private void searchTextBox_TextChanged(object sender, EventArgs e)
        {
            if (_pokemonListViewBackup.Count() != 0)
            {
                pokemonListView.Items.Clear();
                pokemonListView.BeginUpdate();
                pokemonListView.Items.AddRange(_pokemonListViewBackup.Where(i => string.IsNullOrEmpty(searchTextBox.Text) || i.SubItems[1].Text.StartsWith(searchTextBox.Text, StringComparison.OrdinalIgnoreCase)).ToArray());
                pokemonListView.EndUpdate();
            }
        }
    }

    public class ItemComparer : IComparer
    {
        // Column for Comparison
        public int Column { get; set; }

        // Order of Sorting
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

            // Datetime Comparison
            DateTime x1, y1;
            // Parse the two objects passed as a parameter as a DateTime.
            if (!DateTime.TryParse(itemA.SubItems[Column].Text, out x1))
                x1 = DateTime.MinValue;
            if (!DateTime.TryParse(itemB.SubItems[Column].Text, out y1))
                y1 = DateTime.MinValue;
            result = DateTime.Compare(x1, y1);
            if (x1 != DateTime.MinValue && y1 != DateTime.MinValue)
                goto done;

            // Numeric Comparison
            decimal x2, y2;

            if (!Decimal.TryParse(itemA.SubItems[Column].Text, out x2))
                x2 = Decimal.MinValue;
            if (!Decimal.TryParse(itemB.SubItems[Column].Text, out y2))
                y2 = Decimal.MinValue;
            result = Decimal.Compare(x2, y2);
            if (x2 != Decimal.MinValue && y2 != Decimal.MinValue)
                goto done;

            // Alphabetic Comparison
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
