using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using TobbformosPizzaAlkalmazasEgyTabla.Repository;
using TobbformosPizzaAlkalmazasEgyTabla.model;
using System.Diagnostics;

namespace _2019TobbformosMvcPizzaEgyTabla
{
    public partial class FormPizzaFutarKft : Form
    {/// <summary>
     /// Megrendelőket tartalmazó adattábla
     /// </summary>
        private DataTable futarDT = new DataTable();
        /// <summary>
        /// Tárolja a megrendelőket listában
        /// </summary>
        private Repository repoo = new Repository();
        bool ujAdat = false;
        private object dataGridViewMegrendelok;

        private void ujMegsemGombokKezelese()
        {
            if (ujAdat == false)
                return;
            if ((textBoxFutarNev.Text != string.Empty) ||
                (textBoxFuttartel.Text != string.Empty))
            {
                buttonUjMentesf.Visible = true;
                buttonMegsemf.Visible = true;
            }
            else
            {
                buttonUjMentesf.Visible = false;
                buttonMegsemf.Visible = false;
            }
        }
        private void MegrendeloGombokIndulaskor()
        {
            panelFutar.Visible = false;
            panelModositTorolGombokfutar.Visible = false;
            if (dataGridViewFutar.SelectedRows.Count != 0)
                buttonUjFutar.Visible = false;
            else
                buttonUjFutar.Visible = true;
            buttonMegsemf.Visible = false;
            buttonUjMentesf.Visible = false;
        }
        private void beallitGombokatUjMegrendeloMegsemEsMentes()
        {
            if ((dataGridViewFutar.Rows != null) &&
                (dataGridViewFutar.Rows.Count > 0))
                dataGridViewFutar.Rows[0].Selected = true;
            buttonUjMentesf.Visible = false;
            buttonMegsemf.Visible = false;
            panelModositTorolGombokfutar.Visible = true;
            ujAdat = false;

            textBoxFuttartel.Text = string.Empty;
            textBoxFutarNev.Text = string.Empty;

        }
        private void beallitGombokatTextboxokatUjMegrendelonel()
        {
            panelFutar.Visible = true;
            panelModositTorolGombokfutar.Visible = false;
            textBoxFuttartel.Text = string.Empty;
            textBoxFutarNev.Text = string.Empty;
        }

        private void KattintaskorGombok()
        {
            ujAdat = false;
            buttonUjMentesf.Visible = false;
            buttonMegsemf.Visible = false;
            panelModositTorolGombokfutar.Visible = true;
            errorProviderFutartel.Clear();
            errorProviderFutarnev.Clear();
        }
        private void frissitMegrendelőkDGV()
        {
            //Adattáblát feltölti a repoba lévő pizza listából
            futarDT = repoo.getFutarDataTableFromList(); //UnicornsLover
            //Pizza DataGridView-nak a forrása a pizza adattábla
            dataGridViewFutar.DataSource = null;
            dataGridViewFutar.DataSource = futarDT;
        }
        private void dataGridViewFutar_SelectionChanged(object sender, EventArgs e)
        {
            if (ujAdat)
            {
                KattintaskorGombok();
            }
            if (dataGridViewFutar.SelectedRows.Count == 1)
            {
                panelFutar.Visible = true;
                panelModositTorolGombokfutar.Visible = true;
                buttonUjFutar.Visible = true;
                textBoxFutarAzonosito.Text =
                    dataGridViewFutar.SelectedRows[0].Cells[0].Value.ToString();
                textBoxFutarNev.Text =
                    dataGridViewFutar.SelectedRows[0].Cells[1].Value.ToString();
                textBoxFuttartel.Text =
                    dataGridViewFutar.SelectedRows[0].Cells[2].Value.ToString();
            }
            else
            {
                panelFutar.Visible = false;
                panelModositTorolGombokfutar.Visible = false;
                buttonUjFutar.Visible = false;
            }

        }

        private void beallitFutarDGV()
        {
            futarDT.Columns[0].ColumnName = "Azonosító";
            futarDT.Columns[0].Caption = "Megrendelő azonosító";
            futarDT.Columns[1].ColumnName = "Név";
            futarDT.Columns[1].Caption = "Megrendelő név";
            futarDT.Columns[2].ColumnName = "Tel";
            futarDT.Columns[2].Caption = "Telefonszám";

            dataGridViewFutar.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            dataGridViewFutar.ReadOnly = true;
            dataGridViewFutar.AllowUserToDeleteRows = false;
            dataGridViewFutar.AllowUserToAddRows = false;
            dataGridViewFutar.MultiSelect = false;
        }
        private void buttonBeolvasMegrendelok_Click(object sender, EventArgs e)
        {
            //Adatbázisban pizza tábla kezelése
            RepositoryFutarTableDatabase rtp = new RepositoryFutarTableDatabase();
            //A repo-ba lévő pizza listát feltölti az adatbázisból
            repoo.setFutar(rtp.getFutarFromDatabaseTable());
            frissitFutarDGV();
            beallitFutarDGV();
            MegrendeloGombokIndulaskor();
            dataGridViewFutar.SelectionChanged += dataGridViewFutar_SelectionChanged;

        }
        private void buttonTorolFutar_Click(object sender, EventArgs e)
        {
            torolHibauzenetet();
            if ((dataGridViewFutar.Rows == null) ||
                (dataGridViewFutar.Rows.Count == 0))
                return;
            //A felhasználó által kiválasztott sor a DataGridView-ban            
            int sor = dataGridViewFutar.SelectedRows[0].Index;
            if (MessageBox.Show(
                "Valóban törölni akarja a sort?",
                "Törlés",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                //1. törölni kell a listából
                int id = -1;
                if (!int.TryParse(
                         dataGridViewFutar.SelectedRows[0].Cells[0].Value.ToString(),
                         out id))
                    return;
                try
                {
                    repoo.MegrendeloTorlesListabolIDAlapjan(id);
                }
                catch (RepositoryExceptionCantDelete recd)
                {
                    kiirHibauzenetet(recd.Message);
                    Debug.WriteLine("A Megrendelő törlés nem sikerült, nincs a listába!");
                }
                //2. törölni kell az adatbázisból
                RepositoryFutarTableDatabase rdtp = new RepositoryFutarTableDatabase();
                try
                {
                    rdtp.deleteFutarFromDatabase(id);
                }
                catch (Exception ex)
                {
                    kiirHibauzenetet(ex.Message);
                }
                //3. frissíteni kell a DataGridView-t  
                frissitMegrendelőkDGV();
                if (dataGridViewFutar.SelectedRows.Count <= 0)
                {
                    buttonUjFutar.Visible = true;
                }
                beallitFutarDGV();
            }
        }

        private void buttonMegrendeloModosit_Click(object sender, EventArgs e)
        {
            torolHibauzenetet();
            errorProviderFutarnev.Clear();
            errorProviderFutartel.Clear();
            try
            {
                Futar modosult = new Futar(
                    Convert.ToInt32(textBoxFutarAzonosito.Text),
                    textBoxFutarNev.Text,
                    textBoxMegrendeloCim.Text
                    );
                int azonosito = Convert.ToInt32(textBoxFutarAzonosito.Text);
                //1. módosítani a listába
                try
                {
                    repoo.FrissitMegrendelotListaban(azonosito, modosult);
                }
                catch (Exception ex)
                {
                    kiirHibauzenetet(ex.Message);
                    return;
                }
                //2. módosítani az adatbáziba
                RepositoryVevoTableDatabase rdtp = new RepositoryVevoTableDatabase();
                try
                {
                    rdtp.updateVevoInDatabase(azonosito, modosult);
                }
                catch (Exception ex)
                {
                    kiirHibauzenetet(ex.Message);
                }
                //3. módosítani a DataGridView-ban           
                frissitMegrendelőkDGV();
            }
            catch (MegrendeloNevValidation mnv)
            {
                errorProviderFutarnev.SetError(textBoxFutarNev, "Hiba a névben!");
            }
            catch (MegrendeloCimValidation mcv)
            {
                errorProviderMegrendeloCim.SetError(textBoxMegrendeloCim, "Hiba a címben!");
            }
            catch (RepositoryExceptionCantModified recm)
            {
                kiirHibauzenetet(recm.Message);
                Debug.WriteLine("Módosítás nem sikerült, a megrendelő nincs a listába!");
            }
            catch (Exception ex)
            { }
        }
        private void buttonMegrendeloMentes_Click(object sender, EventArgs e)
        {
            torolHibauzenetet();
            errorProviderMegrendeloCim.Clear();
            errorProviderFutarnev.Clear();
            try
            {
                Megrendelo ujM = new Megrendelo(
                    Convert.ToInt32(textBoxFutarAzonosito.Text),
                    textBoxFutarNev.Text,
                    textBoxMegrendeloCim.Text
                    );
                int azonosito = Convert.ToInt32(textBoxMegrendeloAZ.Text);
                //1. Hozzáadni a listához
                try
                {

                    repoo.HozzaAdMegrendeloListahoz(ujM);

                }
                catch (Exception ex)
                {
                    kiirHibauzenetet(ex.Message);
                    return;
                }
                //2. Hozzáadni az adatbázishoz
                RepositoryVevoTableDatabase rdtp = new RepositoryVevoTableDatabase();
                try
                {

                    rdtp.insertVevoToDatabase(ujM);

                }
                catch (Exception ex)
                {
                    kiirHibauzenetet(ex.Message);
                }
                //3. Frissíteni a DataGridView-t
                frissitMegrendelőkDGV();
                beallitGombokatUjMegrendeloMegsemEsMentes();
                if (dataGridViewMegrendelok.SelectedRows.Count == 1)
                {
                    beallitMegrendeloDGV();
                }

            }
            catch (MegrendeloNevValidation mnv)
            {
                errorProviderMegrendeloNev.SetError(textBoxFutarNev, "Hiba a névben!");
            }
            catch (MegrendeloCimValidation mcv)
            {
                errorProviderMegrendeloCim.SetError(textBoxMegrendeloCim, "Hiba a címben!");
            }
            catch (Exception ex)
            {
            }
        }
        private void buttonUjMegrendelo_Click(object sender, EventArgs e)
        {
            ujAdat = true;
            beallitGombokatTextboxokatUjMegrendelonel();
            int ujVevoAz = repoo.getNextMegrendeloID();
            textBoxMegrendeloAZ.Text = ujVevoAz.ToString();
        }

        private void buttonMegrendeloMegse_Click(object sender, EventArgs e)
        {
            beallitGombokatUjMegrendeloMegsemEsMentes();
        }
        private void textBoxMegrendeloNev_TextChanged(object sender, EventArgs e)
        {
            ujMegsemGombokKezelese();
        }

        private void textBoxMegrendeloCim_TextChanged(object sender, EventArgs e)
        {
            ujMegsemGombokKezelese();
        }
    }
}
