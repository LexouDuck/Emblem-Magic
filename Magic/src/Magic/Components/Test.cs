/*
 *  FE Editor - GBA Fire Emblem (U) ROM editor
 *
 *  Copyright (C) 2008-2011 Hextator,
 *  hextator (AIM) hextator@gmail.com (MSN)
 *
 *  Major thanks to Zahlman (AIM/MSN: zahlman@gmail.com) for optimization,
 *  organization and modularity improvements.
 * 
 *  Thanks to Camtech for some contributions to this file.
 *
 *  This program is free software; you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License version 3
 *  as published by the Free Software Foundation
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this program; if not, write to the Free Software
 *  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 *
 *  <Description> This class provides a dialog for editing and saving portraits
 */

package FEditorAdvance;

import java.awt.event.MouseAdapter;
import java.awt.event.MouseEvent;
import java.awt.event.ActionListener;
import java.awt.event.ActionEvent;
import java.io.File;
import javax.imageio.ImageIO;
import javax.swing.event.ChangeListener;
import javax.swing.event.ChangeEvent;
import javax.swing.ImageIcon;
import javax.swing.JLabel;
import javax.swing.JPanel;
import javax.swing.JButton;
import javax.swing.JCheckBox;
import javax.swing.SwingUtilities;
import Controls.ArrayPanel;
import Graphics.GBAImage;
import Controls.PaletteFrame;
import Model.Game;
import Model.PortraitArray;
import java.util.Arrays;

public class PortraitEditor extends Editor<PortraitArray> {
    private JPanel buttonPanel = new JPanel();

    private JPanel displayPanel = new JPanel();
    private JPanel simulationPanel = new JPanel();

    private JLabel displayLabel = new JLabel();
    private JLabel simulationLabel = new JLabel();

    private ArrayPanel<PortraitArray> arrayPanel;

    private JCheckBox eyesClosedBox = new JCheckBox("Eyes always closed");

    private int mouthX, mouthY, eyeX, eyeY;
    private boolean eyesClosed;
    private GBAImage currentImage;

    private JButton revert, save;

    private PaletteFrame palette_editor = new PaletteFrame(
		new ChangeListener() {
        @Override
            public void stateChanged(ChangeEvent e) {
        currentImage.notifyPaletteChangedExternally();
        updateImage();
    }
}
	);

	private JButton addActionButton(String command, ActionListener listener)
{
    JButton button = new JButton(command);
    button.addActionListener(listener);
    buttonPanel.add(button);
    return button;
}

// Clamp mouth and eye coordinates to (0, 8).
private int clamp(int x)
{
    if (x < 0) { return 0; }
    if (x > 8) { return 8; }
    return x;
}

class SimulationMouseTracker extends MouseAdapter
{
    private int clickX, clickY;

    private boolean moveMouth, moveEyes;

		int oldX, oldY;

    @Override
    public void mouseReleased(MouseEvent e) {
        moveMouth = false; moveEyes = false;
    }

    @Override
    public void mousePressed(MouseEvent e) {
        clickX = e.getX(); clickY = e.getY();
        int x = clickX / 8; int y = clickY / 8;
        if (
            x >= mouthX && x < (mouthX + 4) &&
            y >= mouthY && y < (mouthY + 2)
        )
        {
            moveMouth = true;
            oldX = mouthX;
            oldY = mouthY;
        }
        else if (
          arrayHandle.eyesSupported() &&
          x >= eyeX && x < (eyeX + 4) &&
          y >= eyeY && y < (eyeY + 2)
      )
        {
            moveEyes = true;
            oldX = eyeX;
            oldY = eyeY;
        }
    }

    @Override
    public void mouseDragged(MouseEvent e) {
        int dx = e.getX() - clickX; int dy = e.getY() - clickY;
        dx += (dx < 0) ? -4 : 4;
        dy += (dy < 0) ? -4 : 4;
        dx /= 8; dy /= 8;
        if (moveMouth)
        {
            mouthX = clamp(oldX + dx);
            mouthY = clamp(oldY + dy);
        }
        else if (moveEyes)
        {
            eyeX = clamp(oldX + dx);
            eyeY = clamp(oldY + dy);
        }
        updateSimulation();
    }
}

public PortraitEditor(View view)
{
    super(view);

    // Making this a field for refresh handling
    //JPanel displayPanel = new JPanel();
    displayPanel.add(displayLabel);
    // Don't limit the size of this!
    add(displayPanel);

    SimulationMouseTracker tracker = new SimulationMouseTracker();

    // Making this a field for refresh handling
    //JPanel simulationPanel = new JPanel();
    simulationLabel.addMouseListener(tracker);
    simulationLabel.addMouseMotionListener(tracker);
    simulationPanel.add(simulationLabel);
    // Don't limit the size of this!
    add(simulationPanel);

    //JPanel spinnerPanel = new JPanel();

    // FIXME: This should use PortraitArray.maxSize(), but there is
    // no instance available yet.
    arrayPanel = new ArrayPanel<PortraitArray>(this, 0x1, 0xFF, 1);
    arrayPanel.setMaximumSize(arrayPanel.getPreferredSize());
    add(arrayPanel);

    JPanel eyesClosedPanel = new JPanel();
    eyesClosedPanel.add(eyesClosedBox);
    eyesClosedBox.addActionListener
        (new ActionListener() {
                @Override
                public void actionPerformed(ActionEvent e)
{
    eyesClosed = eyesClosedBox.isSelected();
    SwingUtilities.invokeLater(new Runnable() {
                        @Override
                        public void run()
{
    updateSimulation();
}	
					});
				}
			});
		eyesClosedPanel.setMaximumSize(eyesClosedPanel.getPreferredSize());
        add(eyesClosedPanel);

        addActionButton(
			"Load From File...",
            new ActionListener()
{
    @Override
                public void actionPerformed(ActionEvent e)
{
    loadFile();
}
			}
		);
		revert = addActionButton(
			"Revert",
            new ActionListener()
{
    @Override
                public void actionPerformed(ActionEvent e)
{
    refresh();
}
			}
		);
		save = addActionButton(
			"Save",
            new ActionListener()
{
    // Camtech: save method deprecated.
    @Override
                public void actionPerformed(ActionEvent e)
{
    applyChanges();
}
			}
		);
        addActionButton(
			"Save To File...",
            new ActionListener()
{
    @Override
                public void actionPerformed(ActionEvent e)
{
    saveToFile();
}
			}
		);
        addActionButton(
			"Quit",
            new ActionListener()
{
    @Override
                public void actionPerformed(ActionEvent e)
{
    quit();
}
			}
		);

		buttonPanel.setMaximumSize(buttonPanel.getPreferredSize());
        add(buttonPanel);

        // XXX Cuts the widget in half!
        //palette_editor.setMaximumSize(palette_editor.getPreferredSize());
        add(palette_editor);

		//setBounds(0, 0, 360, 420);
	}

	@Override
    public void setup(Game game)
{
    arrayHandle = game.portraitArray();

    arrayPanel.setArray(arrayHandle);

    eyesClosedBox.setEnabled(arrayHandle.eyesSupported());
}

/**
 * Hextator sez: Review "teardown" method comments in Editor
@Override
protected void cleanup() {
    //arrayPanel.setArray(null);
}
**/

@Override
    public boolean changesSaved()
{
    if (!currentImage.sameImageAs(arrayHandle.getPortraitData()))
    {
        return false;
    }
    if (!currentImage.samePaletteAs(arrayHandle.getPortraitData()))
    {
        return false;
    }
    // XXX GUI shouldn't have to be aware of subclasses of the
    // model it wraps
    int[] data = new int[arrayHandle.eyesSupported() ? 5 : 2];
    data[0] = mouthX;
    data[1] = mouthY;
    if (arrayHandle.eyesSupported())
    {
        data[2] = eyeX;
        data[3] = eyeY;
        data[4] = eyesClosed ? 6 : 1;
    }
    // Camtech: This could probably be implemented better.
    int[] savedData = arrayHandle.getMouthAndEyeData();
    if (!Arrays.equals(data, savedData))
    {
        return false;
    }
    return true;
}

@Override
    public void applyChanges()
{
    // Camtech: Just changing stuff around to be consistent.
    arrayHandle.setPortraitData(currentImage);
    saveMouthAndEyeData();
}

private void saveMouthAndEyeData()
{
    int[] data = new int[arrayHandle.eyesSupported() ? 5 : 2];
    data[0] = mouthX;
    data[1] = mouthY;
    if (arrayHandle.eyesSupported())
    {
        data[2] = eyeX;
        data[3] = eyeY;
        data[4] = eyesClosed ? 6 : 1;
    }
    arrayHandle.setMouthAndEyeData(data);
}

@Override
    public void refresh()
{
    int[] data = arrayHandle.getMouthAndEyeData();
    mouthX = data[0];
    mouthY = data[1];
    if (data.length > 2)
    {
        eyeX = data[2];
        eyeY = data[3];
        eyesClosed = data[4] == 6;
        eyesClosedBox.setSelected(eyesClosed);
    }

    currentImage = arrayHandle.getPortraitData();

    try
    {
        palette_editor.setPalette(currentImage.getPalette());
    }
    catch (Exception e)
    {
        // Let this fall through to updateImage(), where the error will
        // be re-detected, causing the icon to be replaced with text.
    }
    // For backwards compatibility: old editors might have written
    // bad values for these fields - or rather, left them
    // uninitialized (with 0xFF bytes).
    mouthX = clamp(mouthX);
    mouthY = clamp(mouthY);
    eyeX = clamp(eyeX);
    eyeY = clamp(eyeY);
    // Ensure that, if the user saves, the changes are actually
    // written to ROM, so that it will work on GBA/emulator.
    saveMouthAndEyeData();

    updateImage();

    displayPanel.setMaximumSize(displayPanel.getPreferredSize());
    simulationPanel.setMaximumSize(simulationPanel.getPreferredSize());
}

private void updateImage()
{
    ImageIcon icon = null;
    try
    {
        //System.out.println(Util.verboseReport("updating image"));
        icon = new ImageIcon(currentImage.getImage());
        //System.out.println(Util.verboseReport("image updated"));
    }
    catch (Exception e)
    {
        CommonDialogs.showCatchErrorDialog(e);
    }
    displayLabel.setIcon(icon);
    displayLabel.setText(
        icon == null ? "Portrait uninitialized or failed to load." : ""
    );

    updateSimulation();
}

private void updateSimulation()
{
    /*
    System.out.println(Util.verboseReport(String.format(
        "simulation data - %d %d %d %d %d",
        eyeX, eyeY, mouthX, mouthY, eyesClosed? 1:0
    )));
    */
    if (currentImage == null)
    {
        simulationLabel.setIcon(null);
        return;
    }

    GBAImage preview;

    // FIXME: pad cards out to the full simulation display size, and have
    // options to set explicitly what is or isn't a card.
    // Hextator: Isn't the former taken care of?
    // Zahlman: I mean when you change the index and go from a full
    // portrait to a card or vice versa, the window shouldn't change size.
    // And no, last I checked it wasn't taken care of.

    if (arrayHandle.isCardSized(currentImage))
    {
        // Remove the rightmost column, which just identifies the bg color.
        preview = new GBAImage(currentImage, 0, 0, 10, 9);
    }
    else
    {
        // Extract the top-left part of the image with the base portrait.
        preview = new GBAImage(currentImage, 0, 0, 12, 10);

        if (arrayHandle.eyesSupported())
        {
            // Place eyes: half-open for "normal eyes" sprites,
            // closed for "eyes always closed" sprites.
            preview.blit(
                currentImage, 12, eyesClosed ? 8 : 6, 4, 2, eyeX, eyeY, true
            );
        }

        // Place mouth - for now, use the 'special' mouth
        preview.blit(currentImage, 12, 10, 4, 2, mouthX, mouthY, true);
    }

    simulationLabel.setIcon(new ImageIcon(preview.getImage()));
}

private void loadFile()
{
    File loadedFile = CommonDialogs.showOpenFileDialog("bitmap");
    try
    {
        System.out.println(Model.Util.verboseReport("got teh imaeg"));
        GBAImage i = new GBAImage(loadedFile);
        if (arrayHandle.isCardSized(i))
        {
            // Clean up the rightmost column.
            currentImage = i;
            currentImage.wipeTiles(10, 0, 1, 9);
            updateImage();
        }
        else if (i.getTileWidth() == 16 && i.getTileHeight() == 14)
        {
            currentImage = i;
            updateImage();
        }
        else if (i.getTileWidth() == 33 && i.getTileHeight() == 6)
        {
            currentImage = arrayHandle.rearrange_legacy(i);
            updateImage();
        }
        else
        {
            CommonDialogs.showGenericErrorDialog(
                "Invalid portrait image size!"
            );
        }
    }
    catch (Exception e)
    {
        e.printStackTrace();
        // Hextator: This is just annoying. Users don't want this.
        // CommonDialogs.showCatchErrorDialog(e);
    }
}

private void saveToFile()
{
    File tempFile = CommonDialogs.showSaveFileDialog("bitmap");
    try
    {
        if (tempFile != null)
        {
            ImageIO.write(currentImage.getImage(), "PNG", tempFile);
        }
    }
    catch (Exception e)
    {
        CommonDialogs.showStreamErrorDialog();
    }
}
}
