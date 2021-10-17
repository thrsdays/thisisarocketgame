#include <SFML/Graphics.hpp>
#include <iostream>

class Ball {

	private:
		sf::Vector2f position;
		float mass;
		sf::Vector2f vector;
		sf::CircleShape shape;

	public:
		// Default constructor
		Ball(){};

		// Initialise a shape at position with no vector
		Ball(sf::Vector2f pos, float m, sf::Vector2f initVector){
			position = pos;
			mass = m;
			vector = initVector;
			sf::CircleShape shape(1.f);

			shape.setOrigin(0.f,0.f);
			shape.setPosition(pos);
			shape.setFillColor(sf::Color::White);
			std::cout << "Scream";
		}

		void draw(sf::RenderWindow& window){
			window.draw(shape);
		}

		// Setter
		void setPosition(sf::Vector2f pos){ position = pos; }
		
		// Getter
		sf::Vector2f getPosition() { return position; }

};


int main()
{
	sf::Vector2f screenSize(800, 800);
	
	sf::RenderWindow window(sf::VideoMode(screenSize.x,screenSize.y), "This is a game", sf::Style::Default);

	// Define starting position
	sf::Vector2f startPoint(screenSize.x / 2, screenSize.y / 2);

	Ball ball1{startPoint, 1, sf::Vector2f(0.f,0.f)};

	bool isMoving {false};
	
	// Run the loop as long as the window is open
	while(window.isOpen())
	{

		// Define a boundary in which the object cannot pass through
		if(ball1.getPosition().x + 1 == screenSize.x || ball1.getPosition().y + 1 == screenSize.y  || ball1.getPosition().x == 0 || ball1.getPosition().y == 0 ){

			isMoving = false;

		} else {

			if(isMoving){
				//shape.move(0.f,1.f);
			}
		}

		// Draw the direction of object vector

		sf::Event event;
		while(window.pollEvent(event))
		{

			switch (event.type)
    		{
    		    // window closed
    		    case sf::Event::Closed:
    		        window.close();
    		        break;
		
    		    // key pressed
    		    case sf::Event::KeyPressed:
    		        if(sf::Keyboard::isKeyPressed(sf::Keyboard::Space)){ // Space to toggle movement
						isMoving = !isMoving;
						std::cout << "Space\n";
					}
    		        break;
		
    		    // we don't process other types of events
    		    default:
    		        break;
    		}
		}
		
		window.clear();
		ball1.draw(window);
		window.display();

	}
	return 0;
}
