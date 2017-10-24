// <copyright file="PanelStyleManagement.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NextStepMVC.Models
{
    using System.Linq;

    public class PanelStyleManagement
    {
        private CMSDbEntities ef;

        public void AddPanelStyle(PanelStyle newStyle)
        {
            using (this.ef = new CMSDbEntities())
            {
                this.ef.PanelStyles.Add(newStyle);
                this.ef.SaveChanges();
            }
        }

        public void UpdatePanelStyle(PanelStyle newStyle, int id)
        {
            if (newStyle == null || id == 1)
            {
                return;
            }

            using (this.ef = new CMSDbEntities())
            {
                var original = this.ef.PanelStyles.Find(id);
                if (original != null)
                {
                    original.color_panel_background = newStyle.color_panel_background;
                    original.color_panel_context = newStyle.color_panel_context;
                    original.color_panel_title = newStyle.color_panel_title;
                    original.font_family_panel_context = newStyle.font_family_panel_context;
                    original.font_family_panel_title = newStyle.font_family_panel_title;
                    original.StyleName = newStyle.StyleName;
                }

                this.ef.SaveChanges();
            }
        }

        public void DeletePanelStyle(int id)
        {
            if (id == 1)
            {
                return;
            }

            using (this.ef = new CMSDbEntities())
            {
                var query2 = from b in this.ef.PanelStyles
                             orderby b.Id
                             select b;
                if (query2.Count() < 2)
                {
                }
                else
                {
                    var ps = this.ef.PanelStyles.Find(id);

                    var query = from b in this.ef.Panels.Include("PanelStyle")
                                where b.StyleSheet == id
                                orderby b.Panel_Id
                                select b;

                    foreach (var item in query)
                    {
                        if (query2.First().Id != id)
                        {
                            item.StyleSheet = query2.First().Id;
                        }
                        else
                        {
                            item.StyleSheet = query2.Last().Id;
                        }
                    }

                    if (ps == null)
                    {                       
                    }
                    else
                    {
                        this.ef.PanelStyles.Remove(ps);
                        this.ef.SaveChanges();
                    }
                }
            }
        }
    }
}