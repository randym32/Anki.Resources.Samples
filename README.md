# Anki Resources SDK Sample applications

This solution contains tutorial applications for the Anki Resources SDK.

## Getting Started

### Documentation

* [Anki Resources SDK Documentation](https://randym32.github.io/Anki.Resources.SDK/)


## Sample Projects

There are three examples for now.

### Example 1 - Playing a Sound

This shows how to open the assets, list the sounds, and to play them.

### Example 2 - Playing a Sprite Sequence

This shows how to open the assets, list the sprite sequence, and to display the
frames on the screen.  This example includes some code to colourize the otherwise
grayscale PNGs (using a "ColorMatrix").  Note: set the colour to white to leave the
sprite sequences with colours alone.

### Example 3 - Playing a Composite Image animation

An example of how to play the composite images -- the image layers which can
contain sprite sequences.  Note: Not composite images reference sprite sequences.
Most of the ones are in the "Weather" groupings.

Note: these composite images often expect to layer sprite sequences on top of
the eyes, which are are rendered by the procedural face module (which isn't
emulated here...

### Example 4 - Recognizing images

An example of how to classify items in the images.

This demo window shows:
   * a live camera feed (from the computers default camera)
   * The cropped feed, so that the result fits within the aspect ratio;
   * The cropped is overlayed with indicators where it things "people" are
     Note: The grid orientation may be wrong.
   
   * The labels of what it sees are listed to the right.  These depend
     on which classifier/detector is used.
 
In general, it does not recognize most things, and is very sensitive to
coloration and contrast.  For instance, it recognizes my black spatula,
but not my chrome-handled spatula, nor bamboo spatula.  (Mobilenet has a
lot of irrelevant things it recognizes, so I've no clue if it is used in
the pet detector/tracker.)

Some tips:
   1. The hand recognizer needs to see all of the fingers and thumb, so you
      will have to spread your fingers a bit, but keep them in the visual
      frame.

  2. The hand recognizer needs the hand to be against a dark surface --
     or perhaps just a contrasting surface.
 
  3. Objects often should be close as possible to the camera, but still in the frame


### Example 5 - Text Substitution

An example of how to employ the text localization strings in the resources/assets.
