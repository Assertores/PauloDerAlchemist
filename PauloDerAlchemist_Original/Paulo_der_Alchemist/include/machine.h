#ifndef MACHINE_H
#define MACHINE_H

#include <variables.h>
#include <math.h>
#include <iostream>
#include <iomanip>

class Machine {
public:
	Machine(InfMach Var);
	InfMach process(Container Prime, Container Secondary);
	void ChangeRatio(int New);
	void Render(unsigned int ContainerSize);

	int InPrimeIndex;
	int InSecondaryIndex;
	int OutPrimeIndex;
	int OutSecondaryIndex;
	bool on;
private:
	std::pair<Material, Material> ComIndex;
	float Ratio;
	InfMach LastProcess;
	float lastEfficiency;
};

#endif // MACHINE_H
