// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using VirtualizingLayout = Microsoft.UI.Xaml.Controls.VirtualizingLayout;
using VirtualizingLayoutContext = Microsoft.UI.Xaml.Controls.VirtualizingLayoutContext;

namespace Homeschool.App.Common;

using System.Collections.Generic;
using System.Collections.Specialized;

using Windows.Foundation;

public class VariedImageSizeLayout : VirtualizingLayout
{
    public double Width { get; set; } = 150;        
    protected override void OnItemsChangedCore(VirtualizingLayoutContext context, object source, NotifyCollectionChangedEventArgs args)
    {
        // The data collection has changed, so the bounds of all the indices are not valid anymore. 
        // We need to re-evaluate all the bounds and cache them during the next measure.
        _mCachedBounds.Clear();
        _mFirstIndex = _mLastIndex = 0;
        _cachedBoundsInvalid = true;
        InvalidateMeasure();
    }

    protected override Size MeasureOverride(VirtualizingLayoutContext context, Size availableSize)
    {
        Rect viewport = context.RealizationRect;

        if (availableSize.Width != _mLastAvailableWidth || _cachedBoundsInvalid)
        {
            UpdateCachedBounds(availableSize);
            _mLastAvailableWidth = availableSize.Width;
        }

        // Initialize column offsets
        int numColumns = (int)(availableSize.Width / Width);
        if (_mColumnOffsets.Count == 0)
        {
            for (int i = 0; i < numColumns; i++)
            {
                _mColumnOffsets.Add(0);
            }
        }

        _mFirstIndex = GetStartIndex(viewport);
        int currentIndex = _mFirstIndex;
        double nextOffset = -1.0;

        // Measure items from start index to when we hit the end of the viewport.
        while (currentIndex < context.ItemCount && nextOffset < viewport.Bottom)
        {
            UIElement child = context.GetOrCreateElementAt(currentIndex);
            child.Measure(new Size(Width, availableSize.Height));

            if (currentIndex >= _mCachedBounds.Count)
            {
                // We do not have bounds for this index. Lay it out and cache it.
                int columnIndex = GetIndexOfLowestColumn(_mColumnOffsets, out nextOffset);
                _mCachedBounds.Add(new(columnIndex * Width, nextOffset, Width, child.DesiredSize.Height));
                _mColumnOffsets[columnIndex] += child.DesiredSize.Height;
            }
            else
            {
                if (currentIndex + 1 == _mCachedBounds.Count)
                {
                    // Last element. Use the next offset.
                    GetIndexOfLowestColumn(_mColumnOffsets, out nextOffset);
                }
                else
                {
                    nextOffset = _mCachedBounds[currentIndex + 1].Top;
                }
            }

            _mLastIndex = currentIndex;
            currentIndex++;
        }

        Size extent = GetExtentSize(availableSize);
        return extent;
    }

    protected override Size ArrangeOverride(VirtualizingLayoutContext context, Size finalSize)
    {
        if (_mCachedBounds.Count > 0)
        {
            for (int index = _mFirstIndex; index <= _mLastIndex; index++)
            {
                UIElement child = context.GetOrCreateElementAt(index);
                child.Arrange(_mCachedBounds[index]);
            }
        }
        return finalSize;
    }

    private void UpdateCachedBounds(Size availableSize)
    {
        int numColumns = (int)(availableSize.Width / Width);
        _mColumnOffsets.Clear();
        for (int i = 0; i < numColumns; i++)
        {
            _mColumnOffsets.Add(0);
        }

        for (int index = 0; index < _mCachedBounds.Count; index++)
        {
            int columnIndex = GetIndexOfLowestColumn(_mColumnOffsets, out double nextOffset);
            double oldHeight = _mCachedBounds[index].Height;
            _mCachedBounds[index] = new(columnIndex * Width, nextOffset, Width, oldHeight);
            _mColumnOffsets[columnIndex] += oldHeight;
        }

        _cachedBoundsInvalid = false;
    }

    private int GetStartIndex(Rect viewport)
    {
        int startIndex = 0;
        if (_mCachedBounds.Count == 0)
        {
            startIndex = 0;
        }
        else
        {
            // find first index that intersects the viewport
            // perhaps this can be done more efficiently than walking
            // from the start of the list.
            for (int i = 0; i < _mCachedBounds.Count; i++)
            {
                Rect currentBounds = _mCachedBounds[i];
                if (currentBounds.Y < viewport.Bottom &&
                    currentBounds.Bottom > viewport.Top)
                {
                    startIndex = i;
                    break;
                }
            }
        }

        return startIndex;
    }

    private int GetIndexOfLowestColumn(List<double> columnOffsets, out double lowestOffset)
    {
        int lowestIndex = 0;
        lowestOffset = columnOffsets[lowestIndex];
        for (int index = 0; index < columnOffsets.Count; index++)
        {
            double currentOffset = columnOffsets[index];
            if (lowestOffset > currentOffset)
            {
                lowestOffset = currentOffset;
                lowestIndex = index;
            }
        }

        return lowestIndex;
    }

    private Size GetExtentSize(Size availableSize)
    {
        double largestColumnOffset = _mColumnOffsets[0];
        for (int index = 0; index < _mColumnOffsets.Count; index++)
        {
            double currentOffset = _mColumnOffsets[index];
            if (largestColumnOffset < currentOffset)
            {
                largestColumnOffset = currentOffset;
            }
        }

        return new(availableSize.Width, largestColumnOffset);
    }

    int _mFirstIndex = 0;
    int _mLastIndex = 0;
    double _mLastAvailableWidth = 0.0;
    List<double> _mColumnOffsets = new();
    List<Rect> _mCachedBounds = new();
    private bool _cachedBoundsInvalid = false;
}