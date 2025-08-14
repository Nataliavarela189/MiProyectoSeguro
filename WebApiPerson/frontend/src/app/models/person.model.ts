export interface Person {
  id: number;
  name: string;
  identification: string;
  age: number;
  gender: string;
  isActive: boolean;
  drives: boolean;
  wearsGlasses: boolean;
  hasDiabetes: boolean;
  otherDiseases?: string;
}
