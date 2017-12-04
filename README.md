# leapmotion-midi
![Alt Text](https://static1.squarespace.com/static/57bf65a78419c24a012e3072/t/588ea35f03596e0fcbcb12b4/1498925412894/)

## This is project which involves a leap motion sensor as midi device.
The leap motion sensor uses optical sensors and infrared light which operates through close proximity with high precision for tracking and recognizing hands and fingers.
The API released by the Leap motion World allows developers to program and customize some functionality that can performed by the sensor. 
API stands for Application Program Interface, it contains predefined functions or methods (in the case of C#) that are generally saved as a library.
The leap motion SDK (software development kit) provides a native interface for getting data frames from the leap motion service. The leap motion SDK also provides tracking data via web socket interface using a sever client approach. 
The native interface grants to the developer access data frames from the optical sensors, initiate connection between an operating system and a connected leap motion via USB. 
The native interface is also a dynamic library that allows a developer to create leap-enabled applications. 
The focus of this project is to track hand movement in the field of view (FOV) of the leap motion sensor. 
The track parameters of the Leap motion API were the Hand and Finger data.

The parameters computed from the hand data frame are of the following:
1. Hand type: first and second hand that depends on the first hand recognized by the leap motion sensor per frame.
2. Palm position: measuring the center of the user palm from the origin of the leap motion’s coordinate system.
3. Grab strength: measuring the probability of a grab hand pose or an opened palm.
4. Palm velocity: measuring how fast the user’s hand move per second.
5. Palm rotation or orientation: computation of the roll, yaw and pitch angle of a palm with respect to the orientation of a palm in a 3D space. Rotation of a palm from front to back axis are computed for the roll. Side-to-side axis rotation are computed for the pitch, and rotation about the vertical axis are computed for yaw.

## Grab strength
![Alt Text](http://blog.leapmotion.com/wp-content/uploads/2014/08/grab-strength.gif)

## Visualizer of hand and finger per video frame
![Alt Text](http://blog.leapmotion.com/wp-content/uploads/2014/08/visualizer.jpg) 

The parameters computed from the finger data frame are of the following;
1. Type of finger: recognizing the index finger and middle finger of the user’s finger.
2. Finger Direction (vector): computing the vector coordinates of the two fingers.
The purposes of computing these two parameters from each frame captured was to recognize if the user’s fingers are spread in the FOV of the sensor. The coordinate vectors from the index finger and middle finger are used for calculating the product Euclidean magnitudes (length) and through geometry approach the dot product of the two length are calculated in order to find the arccosine of the angle between the two fingers.
## The angle between the index and middle fingers
![Alt Text](https://d1zyaropqswkf6.cloudfront.net/uploads/library/thumbnail_image/c2822955-971b-4872-b8c1-a8468d293064.gif)

## Musical Instrument Digital Interface(MIDI) protocol
Computed values from the hands and fingers data frame are mapped into the resolution of 1- 127. This was done because midi protocol used to communicate with the external application Ableton.

    

Channel Command | MIDI Channel | Control Number data byte 1 | Value byte 2
--- | --- | --- | ---
0xB0 | 1-15 | 1-127 | 1-127  


[This link to videos recordings during development stages](https://www.youtube.com/playlist?list=PLkPgAczs7Y9DzX166tG1_RdgPS_pzC003)
