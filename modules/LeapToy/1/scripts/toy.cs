//-----------------------------------------------------------------------------
// Copyright (c) 2013 GarageGames, LLC
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to
// deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
// sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// IN THE SOFTWARE.
//-----------------------------------------------------------------------------

function LeapToy::createGesturesLevel( %this )
{
    // Create background.
    %this.createBackground();
    
    // Create the ground.
    %this.createGround();

    // Create the pyramid.
    %this.createPyramid();

    // Create a ball.
    %this.createBall();

    // Create circle gesture visual.
    %this.createCircleSprite();
    
    %this.selectedObjects.clear();
    
    GestureMap.push();
}

//-----------------------------------------------------------------------------

function LeapToy::createBreakoutLevel( %this )
{
    echo("BREAKOUT!");
    
    LeapMap.push();
}

//-----------------------------------------------------------------------------

function LeapToy::createBackground( %this )
{    
    // Create the sprite.
    %background = new Sprite();
    
    // Set the sprite as "static" so it is not affected by gravity.
    %background.setBodyType( static );
       
    // Set the position.
    %background.Position = "0 0";

    // Set the size.        
    %background.Size = "40 30";
    
    // Set to the furthest background layer.
    %background.SceneLayer = 31;
    
    // Set an image.
    %background.Image = "LeapToy:menuBackground";

    %background.createEdgeCollisionShape( -20, -15, -20, 15 );
    %background.createEdgeCollisionShape( 20, -15, 20, 15 );
    %background.createEdgeCollisionShape( -20, 15, 20, 15 );

    // Add the sprite to the scene.
    SandboxScene.add( %background );
    
    %this.createCircles();
}

//-----------------------------------------------------------------------------

function LeapToy::createCircles( %this )
{
    // Create the sprite.
    %circleOne = new Sprite();
    %circleTwo = new Sprite();
    
    // Set the sprite as "static" so it is not affected by gravity.
    %circleOne.BodyType = "Kinematic";
    %circleTwo.BodyType = "Kinematic";
       
    // Set the position.
    %circleOne.Position = "15 10";
    %circleTwo.Position = "15 10";
    
    // Set the size.        
    %circleOne.Size = "15 15";
    %circleTwo.Size = "15 15";
    
    // Set to the furthest background layer.
    %circleOne.SceneLayer = 30;
    %circleTwo.SceneLayer = 30;
    
    // Set an image.
    %circleOne.Image = "LeapToy:complexCircle";
    %circleTwo.Image = "LeapToy:simpleCircle";
    
    %circleOne.AngularVelocity = 15;
    %circleTwo.AngularVelocity = -15;
    
    // Add the sprite to the scene.
    SandboxScene.add( %circleOne );
    SandboxScene.add( %circleTwo );
}

//-----------------------------------------------------------------------------

function LeapToy::createGround( %this )
{
    // Create the ground
    %ground = new Scroller();
    %ground.setBodyType("static");
    %ground.Image = "LeapToy:window";
    %ground.setPosition(0, -12);
    %ground.SceneLayer = 11;
    %ground.setSize(LeapToy.GroundWidth, 6);
    %ground.setRepeatX(LeapToy.GroundWidth / 40);   
    %ground.createEdgeCollisionShape(LeapToy.GroundWidth/-2, 3, LeapToy.GroundWidth/2, 3);
    SandboxScene.add(%ground);    
}

//-----------------------------------------------------------------------------

function LeapToy::createPyramid( %this )
{
    // Fetch the block count.
    %blockCount = LeapToy.BlockCount;

    // Sanity!
    if ( %blockCount < 2 )
    {
        echo( "Cannot have a pyramid block count less than two." );
        return;
    }

    // Set the block size.
    %blockSize = LeapToy.BlockSize;

    // Calculate a block building position.
    %posx = %blockCount * -0.5 * %blockSize;
    %posy = -8.8 + (%blockSize * 0.5);

    // Build the stack of blocks.
    for( %stack = 0; %stack < %blockCount; %stack++ )
    {
        // Calculate the stack position.
        %stackIndexCount = %blockCount - (%stack*2);
        %stackX = %posX + ( %stack * %blockSize );
        %stackY = %posY + ( %stack * %blockSize );

        // Build the stack.
        for ( %stackIndex = 0; %stackIndex < %stackIndexCount; %stackIndex++ )
        {
            // Calculate the block position.
            %blockX = %stackX + (%stackIndex*%blockSize);
            %blockY = %stackY;
            %blockFrames = "0 2 4 6";
            %randomNumber = getRandom(0, 4);
            
            // Create the sprite.
            %obj = new Sprite()
            {
                class = "Block";
                flippd = false;
            };
            
            %obj.setPosition( %blockX, %blockY );
            %obj.setSize( %blockSize );
            %obj.setImage( "LeapToy:objectsBlocks" );            
            %obj.setImageFrame( getWord(%blockFrames, %randomNumber) );
            %obj.setDefaultFriction( 1.0 );
            %obj.createPolygonBoxCollisionShape( %blockSize, %blockSize );

            // Add to the scene.
            SandboxScene.add( %obj );
        }
    }
}

//-----------------------------------------------------------------------------

function LeapToy::createBall( %this )
{
    // Create the ball.
    %ball = new Sprite();
    %ball.Position = "5 5";
    %ball.Size = 4;
    %ball.Image = "LeapToy:widgetBall";        
    %ball.setDefaultDensity( 1 );
    %ball.setDefaultRestitution( 0.5 );
    %ball.createCircleCollisionShape( 1.8 );

    %this.ball = %ball;

    // Add to the scene.
    SandboxScene.add( %ball );
}

//-----------------------------------------------------------------------------

function LeapToy::createCircleSprite( %this )
{
    // Create the circle.
    %circle = new Sprite();
    %circle.Position = "-10 5";
    %circle.setBodyType("Kinematic");
    %circle.Size = 5;
    %circle.Image = "LeapToy:circleThree";
    %circle.Visible = false;
    %circle.AngularVelocity = 180;
    %this.circleSprite = %circle;

    // Add to the scene.
    SandboxScene.add( %circle );
}

//-----------------------------------------------------------------------------
// This will be called when the user presses the spacebar
//
// %val - Will be true if the spacebar is down, false if it was released
function LeapToy::pickSprite( %this, %val )
{
    // Find out where the cursor is located, then convert to world coordinates
    %cursorPosition = Canvas.getCursorPos();
    %worldPosition = SandboxWindow.getWorldPoint(%cursorPosition);
    
    // If true, force an onTouchDown for the Sandbox input listener
    // If false, force an onTouchUp for the Sandbox input listener    
    if (%val)
    {
        Sandbox.InputController.onTouchDown(0, %worldPosition);
    }
    else
    {
        Sandbox.InputController.onTouchUp(0, %worldPosition);
    }
}

//-----------------------------------------------------------------------------
// This will be callsed when the user makes a circle gesture with a Leap Motion
//
// %radius - How large the circle gesture was
// %isClockwise - True if the motion was clockwise
function LeapToy::showCircleSprite( %this, %radius, %isClockwise )
{
    // If it isn't currently visible, show it
    if (!%this.circleSprite.visible)
    {
        %this.circleSprite.visible = true;
        
        // Find out where the cursor is currenty location
        %worldPosition = SandboxWindow.getWorldPoint(Canvas.getCursorPos());
        
        // Move the circle to that spot. This should be where the circle
        // gesture first started
        %this.circleSprite.position = %worldPosition;
    }
    
    // Resize the circle based on how big the radius was
    %this.circleSprite.size = %radius;
    
    // Rotate to the right if the circle is clockwise, left otherwise
    if (%isClockwise)
        %this.circleSprite.AngularVelocity = -180;
    else
        %this.circleSprite.AngularVelocity = 180;
}

//-----------------------------------------------------------------------------
// This will be called when the user stops making a circle gesture
function LeapToy::hideCircleSprite( %this )
{
    // Hide the sprite
    %this.circleSprite.visible = 0;
}

//-----------------------------------------------------------------------------
// This will be called when either the hand position or hand rotation is being tracked
//
// %horizontalSpeed - How fast to move in the X-axis
// %verticalSpeed - How fast to move in the Y-axis
// %angularSpeed - How fast to rotate on the Z-axis
function LeapToy::accelerateBall( %this, %horizontalSpeed, %verticalSpeed )
{
    %this.ball.setLinearVelocity(%horizontalSpeed, %verticalSpeed);
}

//-----------------------------------------------------------------------------
// This is called when a user makes a swipe gesture with the Leap Motion
//
// %position - Where to spawn the asteroid
// %direction - 3 point vector based on the direction the finger swiped
// %speed - How fast the user's finger moved. Values will be quite large
function LeapToy::createAsteroid( %this, %position, %direction, %speed )
{
    // Size of the asteroid.
    %size = 3;
    
    // Reduce the speed of the swipe so it can be used for a reasonable
    // velocity in T2D.
    %reducedSpeed = mClamp((%speed / 8), 0, 55);
    %velocity = vectorScale(%direction, %reducedSpeed);

    // Create an asteroid.
    %object = new Sprite()
    {
        class = "Asteroid";
    };

    %object.Position = %position;
    %object.CollisionCallback = true;
    %object.Size = %size;
    %object.SceneLayer = 8;
    %object.Image = "ToyAssets:Asteroids";
    %object.ImageFrame = getRandom(0,3);
    %object.setDefaultDensity( 3 );
    %object.createCircleCollisionShape( %size * 0.4 );
    %object.setLinearVelocity( %velocity._0, %velocity._1 );
    %object.setAngularVelocity( getRandom(-90,90) );
    %object.setLifetime( 10 );
    SandboxScene.add( %object );

    // Create fire trail.
    %player = new ParticlePlayer();
    %player.Particle = "ToyAssets:bonfire";
    %player.Position = %object.Position;
    %player.EmissionRateScale = 3;
    %player.SizeScale = 2;
    %player.SceneLayer = 0;
    %player.setLifetime( 10 );
    SandboxScene.add( %player );
    %jointId = SandboxScene.createRevoluteJoint( %object, %player );
    SandboxScene.setRevoluteJointLimit( %jointId, 0, 0 );

    // Assign the trail to the asteroid, used for cleanup later
    %object.Trail = %player;
}

//-----------------------------------------------------------------------------
// Called when an object with a class of "Asteroid" collides with another body.
// In this toy, it will delete the asteroid and create an explosion
//
// %object - What the asteroid collided with
// %collisionDetails - Information about the collision
function Asteroid::onCollision( %this, %object, %collisionDetails )
{
    // Create explosion.
    %player = new ParticlePlayer();
    %player.BodyType = static;
    %player.Particle = "ToyAssets:impactExplosion";
    %player.Position = %this.Position;
    %player.SceneLayer = 0;
    SandboxScene.add( %player );

    %controller = new PointForceController();
    %controller.setControlLayers( 8 ); // Only affect asteroids.
    %controller.Radius = 5;
    %controller.Force = -15;
    %controller.NonLinear = true;
    %controller.LinearDrag = 0.1;
    %controller.AngularDrag = 0;
    SandboxScene.Controllers.add( %controller );

    %controller.Position = %this.Position;
    %id = %controller.getID();
    schedule(100, 0, "%id.safeDelete();");
        
    // Delete the asteroid.
    %this.Trail.LinearVelocity = 0;
    %this.Trail.AngularVelocity = 0;
    %this.Trail.safeDelete();
    %this.safeDelete();
}

//-----------------------------------------------------------------------------

function LeapToy::grabObjectsInCircle( %this, %radius )
{
    %worldPosition = SandboxWindow.getWorldPoint(Canvas.getCursorPos());
    %picked = SandboxScene.pickCircle(%worldPosition, %radius);

    // Finish if nothing picked.
    if ( %picked $= "" )
        return;

    // Fetch the pick count.
    %pickCount = %picked.Count;

    for( %n = 0; %n < %pickCount; %n++ )
    {
        // Fetch the picked object.
        %pickedObject = getWord( %picked, %n );

        // Skip if the object is static.
        if ( %pickedObject.getBodyType() $= "static" || %pickedObject.getBodyType() $= "kinematic")
            continue;
        
        if (%pickedObject.class $= "block")
            %pickedObject.flipFrame();
    }
}

//-----------------------------------------------------------------------------

function Block::flipFrame( %this )
{
    %currentFrame = %this.getImageFrame();
    
    if (%this.flipped == true)
    {
        %newFrame = %currentFrame - 1;
        %this.flipped = false;
        LeapToy.selectedObjects.remove(%this);
    }
    else
    {
        %newFrame = %currentFrame + 1;
        %this.flipped = true;
        LeapToy.selectedObjects.add(%this);
    }

    %this.setImageFrame(%newFrame);
    
}

//-----------------------------------------------------------------------------

function LeapToy::createNewBlock( %this )
{
    %worldPosition = SandboxWindow.getWorldPoint(Canvas.getCursorPos());

    // Create the sprite.
    %obj = new Sprite();
    %obj.setPosition( %worldPosition );
    %obj.setSize( LeapToy.blockSize );
    %obj.setImage( "ToyAssets:blocks" );
    %obj.setImageFrame( getRandom(0,55) );
    %obj.setDefaultFriction( 1.0 );
    %obj.createPolygonBoxCollisionShape( LeapToy.blockSize, LeapToy.blockSize );

    // Add to the scene.
    SandboxScene.add( %obj );
}

//-----------------------------------------------------------------------------

function LeapToy::deleteSelectedObjects( %this )
{
    while (%this.selectedObjects.getCount() > 0)
        %this.selectedObjects.getObject(0).delete();
}