//*********************************************************
//
// Copyright (c) Microsoft. All rights reserved.
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************



// The data model defined by this file serves as a representative example of a strongly-typed
// model.  The property names chosen coincide with data bindings in the standard item templates.
//
// Applications may use this model as a starting point and build on it, or discard it entirely and
// replace it with something appropriate to their needs. If using this model, you might improve app
// responsiveness by initiating the data loading task in the code behind for App.xaml when the app
// is first launched.

namespace Homeschool.App.Data;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using Windows.Data.Json;
using Windows.Storage;

/// <summary>
/// Generic item data model.
/// </summary>
public class ControlInfoDataItem
{
    public ControlInfoDataItem(string uniqueId, string title, string subtitle, string imagePath, string badgeString, string description, string content, bool isNew, bool isUpdated, bool isPreview)
    {
        UniqueId = uniqueId;
        Title = title;
        Subtitle = subtitle;
        Description = description;
        ImagePath = imagePath;
        BadgeString = badgeString;
        Content = content;
        IsNew = isNew;
        IsUpdated = isUpdated;
        IsPreview = isPreview;
        Docs = new();
        RelatedControls = new();
    }

    public string UniqueId { get; private set; }
    public string Title { get; private set; }
    public string Subtitle { get; private set; }
    public string Description { get; private set; }
    public string ImagePath { get; private set; }
    public string BadgeString { get; private set; }
    public string Content { get; private set; }
    public bool IsNew { get; private set; }
    public bool IsUpdated { get; private set; }
    public bool IsPreview { get; private set; }
    public ObservableCollection<ControlInfoDocLink> Docs { get; private set; }
    public ObservableCollection<string> RelatedControls { get; private set; }

    public override string ToString()
        => Title;
}

public class ControlInfoDocLink
{
    public ControlInfoDocLink(string title, string uri)
    {
        Title = title;
        Uri = uri;
    }
    public string Title { get; private set; }
    public string Uri { get; private set; }
}


/// <summary>
/// Generic group data model.
/// </summary>
public class ControlInfoDataGroup
{
    public ControlInfoDataGroup(string uniqueId, string title, string subtitle, string imagePath, string description)
    {
        UniqueId = uniqueId;
        Title = title;
        Subtitle = subtitle;
        Description = description;
        ImagePath = imagePath;
        Items = new();
    }

    public string UniqueId { get; private set; }
    public string Title { get; private set; }
    public string Subtitle { get; private set; }
    public string Description { get; private set; }
    public string ImagePath { get; private set; }
    public ObservableCollection<ControlInfoDataItem> Items { get; private set; }

    public override string ToString()
        => Title;
}

/// <summary>
/// Creates a collection of groups and items with content read from a static json file.
///
/// ControlInfoSource initializes with data read from a static json file included in the
/// project.  This provides sample data at both design-time and run-time.
/// </summary>
public sealed class ControlInfoDataSource
{
    private static readonly object _lock = new();

    #region Singleton

    private static ControlInfoDataSource _instance;

    public static ControlInfoDataSource Instance => ControlInfoDataSource._instance;

    static ControlInfoDataSource()
        => ControlInfoDataSource._instance = new();

    private ControlInfoDataSource() { }

    #endregion

    private IList<ControlInfoDataGroup> _groups = new List<ControlInfoDataGroup>();
    public IList<ControlInfoDataGroup> Groups => _groups;

    public async Task<IEnumerable<ControlInfoDataGroup>> GetGroupsAsync()
    {
        await ControlInfoDataSource._instance.GetControlInfoDataAsync().ConfigureAwait(false);

        return ControlInfoDataSource._instance.Groups;
    }

    public async Task<ControlInfoDataGroup> GetGroupAsync(string uniqueId)
    {
        await ControlInfoDataSource._instance.GetControlInfoDataAsync().ConfigureAwait(false);
        // Simple linear search is acceptable for small data sets
        var matches = ControlInfoDataSource._instance.Groups.Where((group) => group.UniqueId.Equals(uniqueId));
        if (matches.Count() == 1)
        {
            return matches.First();
        }

        return null;
    }

    public async Task<ControlInfoDataItem> GetItemAsync(string uniqueId)
    {
        await ControlInfoDataSource._instance.GetControlInfoDataAsync().ConfigureAwait(false);
        // Simple linear search is acceptable for small data sets
        var matches = ControlInfoDataSource._instance.Groups.SelectMany(group => group.Items).Where((item) => item.UniqueId.Equals(uniqueId));
        if (matches.Count() > 0)
        {
            return matches.First();
        }

        return null;
    }

    public async Task<ControlInfoDataGroup> GetGroupFromItemAsync(string uniqueId)
    {
        await ControlInfoDataSource._instance.GetControlInfoDataAsync().ConfigureAwait(false);
        var matches = ControlInfoDataSource._instance.Groups.Where((group) => group.Items.FirstOrDefault(item => item.UniqueId.Equals(uniqueId)) is not null);
        if (matches.Count() == 1)
        {
            return matches.First();
        }

        return null;
    }

    private async Task GetControlInfoDataAsync()
    {
        lock (ControlInfoDataSource._lock)
        {
            if (Groups.Count() != 0)
            {
                return;
            }
        }

        Uri dataUri = new("ms-appx:///DataModel/ControlInfoData.json");

        var file = await StorageFile.GetFileFromApplicationUriAsync(dataUri);
        var jsonText = await FileIO.ReadTextAsync(file);

        var jsonObject = JsonObject.Parse(jsonText);
        var jsonArray = jsonObject["Groups"].GetArray();

        lock (ControlInfoDataSource._lock)
        {
            foreach (JsonValue groupValue in jsonArray)
            {
                var groupObject = groupValue.GetObject();
                ControlInfoDataGroup group = new(groupObject["UniqueId"].GetString(),
                    groupObject["Title"].GetString(),
                    groupObject["Subtitle"].GetString(),
                    groupObject["ImagePath"].GetString(),
                    groupObject["Description"].GetString());

                foreach (JsonValue itemValue in groupObject["Items"].GetArray())
                {
                    var itemObject = itemValue.GetObject();

                    string badgeString = null;

                    var isNew = itemObject.ContainsKey("IsNew") ? itemObject["IsNew"].GetBoolean() : false;
                    var isUpdated = itemObject.ContainsKey("IsUpdated") ? itemObject["IsUpdated"].GetBoolean() : false;
                    var isPreview = itemObject.ContainsKey("IsPreview") ? itemObject["IsPreview"].GetBoolean() : false;

                    if (isNew)
                    {
                        badgeString = "New";
                    }
                    else if (isUpdated)
                    {
                        badgeString = "Updated";
                    }
                    else if (isPreview)
                    {
                        badgeString = "Preview";
                    }

                    ControlInfoDataItem item = new(itemObject["UniqueId"].GetString(),
                        itemObject["Title"].GetString(),
                        itemObject["Subtitle"].GetString(),
                        itemObject["ImagePath"].GetString(),
                        badgeString,
                        itemObject["Description"].GetString(),
                        itemObject["Content"].GetString(),
                        isNew,
                        isUpdated,
                        isPreview);

                    if (itemObject.ContainsKey("Docs"))
                    {
                        foreach (JsonValue docValue in itemObject["Docs"].GetArray())
                        {
                            var docObject = docValue.GetObject();
                            item.Docs.Add(new(docObject["Title"].GetString(), docObject["Uri"].GetString()));
                        }
                    }

                    if (itemObject.ContainsKey("RelatedControls"))
                    {
                        foreach (JsonValue relatedControlValue in itemObject["RelatedControls"].GetArray())
                        {
                            item.RelatedControls.Add(relatedControlValue.GetString());
                        }
                    }

                    group.Items.Add(item);
                }

                if (!Groups.Any(g => g.Title == group.Title))
                {
                    Groups.Add(group);
                }
            }
        }
    }
}