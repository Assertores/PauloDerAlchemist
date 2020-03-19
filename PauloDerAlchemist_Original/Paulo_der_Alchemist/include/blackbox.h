#ifndef BLACKBOX_H
#define BLACKBOX_H

#include <variables.h>
#include <machine.h>
#include <map>
#include <vector>
#include <iomanip>

class Blackbox{
public:
	Blackbox(Blackbox* Parent);
	~Blackbox();
	Blackbox* input(std::vector<std::string> input);
	void process();
	void render();
	void AddIn(Container* New);
	bool DeleteIn(Container* New);
	void AddOut(Container* New);
	bool DeleteOut(Container* New);

	std::map<Container*, Container*> In;
	std::map<Container*, Container*> Out;
private:
	Blackbox* Father;
	std::map<int, Container> ContainerAtlas;
	int LastContainerIndex;
	std::map<int, Machine> MachineAtlas;
	int LastMachineIndex;
	std::map<std::string, Blackbox*> BlackboxAtlas;
};

#endif // BLACKBOX_H
