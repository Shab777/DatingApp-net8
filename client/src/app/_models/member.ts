import { Photo } from "./photo"

export interface Member {
    map(arg0: (m: any) => any): Member[]
    id: number
    userName: string
    age: number
    photoUrl: string
    knownAs: string
    created: Date
    lastActive: Date
    introduction: any
    interests: string 
    lookingFor: string
    city: string
    country: string
    photos: Photo[]
  }
  
  