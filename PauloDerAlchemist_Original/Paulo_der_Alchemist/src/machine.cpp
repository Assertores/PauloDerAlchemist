#include "machine.h"

Machine::Machine(InfMach Var){
	OutPrimeIndex = (int)Var.PrimeUsage;
	OutSecondaryIndex = (int)Var.SecondaryUsage;
	InPrimeIndex = (int)Var.Prime.second;
	InSecondaryIndex = (int)Var.Secondary.second;

	ComIndex.first = Var.Prime.first;
	ComIndex.second = Var.Secondary.first;
	Ratio = 50;
	LastProcess.Prime.first = Var.Prime.first;
	LastProcess.Prime.second = 0;
	LastProcess.Secondary.first = Var.Secondary.first;
	LastProcess.Secondary.second = 0;
	LastProcess.PrimeUsage = 0;
	LastProcess.SecondaryUsage = 0;
	lastEfficiency = 0;
	on = true;
	Container t1;
	Container t2;
	t1.Amount = t2.Amount = 50;
	t1.Content = Var.Prime.first;
	t2.Content = Var.Secondary.first;
	process(t1, t2);
}

InfMach Machine::process(Container Prime, Container Secondary){
	ComIndex.first = Prime.Content;
	ComIndex.second = Secondary.Content;
	Combination *Com = &ComAtlas[ComIndex];
	InfMach ret;
	if(Prime.Amount == 0 || Secondary.Amount == 0 || Ratio <= 0 || Ratio >= 100 || !on){
		ret.PrimeUsage = 0;
		ret.SecondaryUsage = 0;
		ret.Prime.second = 0;
		ret.Secondary.second = 0;
		LastProcess = ret;
		return ret;
	}
	float Efficiency;
	float Out;
	if((float)Com->XMax < ((float)Ratio/100)*16){
		Efficiency = -0.5*(Com->YMax+1)*cos((((Ratio/100)*16 - 16) * M_PI)/(Com->XMax - 16))+0.5*(Com->YMax+1);
	}else{
		Efficiency = -0.5*(Com->YMax+1)*cos(((Ratio/100)*16 * M_PI)/Com->XMax)+0.5*(Com->YMax+1);
	}
	lastEfficiency = Efficiency;

	Out = (exp2(-(((Ratio/100)*16 - Com->XCurve) * ((Ratio/100)*16 - Com->XCurve))/(10*Com->DevCurve))*Com->AmpCurve+Com->YCurve)/2 + 8;

	ret.Prime.first = Com->OutPrime;
	ret.Secondary.first = Com->OutSecondary;
	ret.PrimeUsage = Efficiency/16 * MaxOutput * Ratio/100;
	ret.SecondaryUsage = (Efficiency/16 * MaxOutput) - ret.PrimeUsage;

	float usage = (float)Prime.Amount/ret.PrimeUsage;
	if ((float)Secondary.Amount/ret.SecondaryUsage < usage){
		usage = (float)Secondary.Amount/ret.SecondaryUsage;
	}
	if (usage > 1){
		usage = 1;
	}
#ifndef NDEBUG
	std::cout << Prime.Amount << "/" << ret.PrimeUsage << " (" << (float)Prime.Amount/ret.PrimeUsage << ") >? " << Secondary.Amount << "/" << ret.SecondaryUsage << " (" << (float)Secondary.Amount/ret.SecondaryUsage << ")" << std::endl;
	std::cout << "usage = " << usage << std::endl;
#endif

	ret.PrimeUsage *= usage;
	ret.SecondaryUsage *= usage;
	ret.Prime.second = (Out * Efficiency)/256 * MaxOutput * usage;
	ret.Secondary.second =((16-Out) * Efficiency)/256 * MaxOutput * usage;

	LastProcess = ret;
	return ret;
}

void Machine::ChangeRatio(int New){
	if (New < 0){
		Ratio = 0;
		return;
	}
	if (New > 100){
		Ratio = 100;
		return;
	}
	Ratio = New;
}

void Machine::Render(unsigned int ContainerSize){
	Combination *Com = &ComAtlas[ComIndex];

	for (unsigned int i = 0; i < ContainerSize; i++){
		std::cout << OU << " ";
	}
	std::cout << UR << LOR << LR << LR << LR << LR << LR << LR << LR << LR << LR << LR << LOR << LR << LR << LR << LR << LR << LR << LR << LR << LR << LR << LU;
	std::cout << " State: ";
	if(on)
		std::cout << "on";
	else
		std::cout << "off";
	std::cout << std::endl;
	for (unsigned int i = 0; i < ContainerSize; i++){
		std::cout << OU << " ";
	}
	std::cout << OU << std::left << std::setw(11) << GetName(ComIndex.first) << std::setw(11) << GetName(ComIndex.second) << OU << std::endl;

	for (unsigned int i = 0; i < ContainerSize; i++){
		std::cout << OU << " ";
	}
	std::cout << OU << std::left << std::setw(11) << LastProcess.PrimeUsage << std::setw(11) << LastProcess.SecondaryUsage << OU << std::endl;

	for (unsigned int i = 0; i < ContainerSize; i++){
		std::cout << OU << " ";
	}
	char buf[10];
	std::sprintf(buf, "%.2f%%", Ratio);
	std::cout << OU << std::left << std::setw(11) << buf << std::setw(11) << lastEfficiency << OU << std::endl;

	for (unsigned int i = 0; i < ContainerSize; i++){
		std::cout << OU << " ";
	}
	std::cout << OU << std::left << std::setw(11) << LastProcess.Prime.second << std::setw(11) << LastProcess.Secondary.second << OU << std::endl;

	for (unsigned int i = 0; i < ContainerSize; i++){
		std::cout << OU << " ";
	}
	std::cout << OU << std::left << std::setw(11) << GetName(Com->OutPrime) << std::setw(11) << GetName(Com->OutSecondary) << OU  << std::endl;

	for (unsigned int i = 0; i < ContainerSize; i++){
		std::cout << OU << " ";
	}
	std::cout << OR << LUR << LR << LR << LR << LR << LR << LR << LR << LR << LR << LR << LUR << LR << LR << LR << LR << LR << LR << LR << LR << LR << LR << LO << std::endl;
}
