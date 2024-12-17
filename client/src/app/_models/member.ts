import { Photo } from "./photo"

export interface Member {
    id: number
    userName: string
    age: number
    photoUrl: string
    knownAs: string
    created: Date
    lastActive: Date
    introudction: any
    interests: string
    lookingFor: string
    city: string
    country: string
    photos: Photo[]
  }
  
  