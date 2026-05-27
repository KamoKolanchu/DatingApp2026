import { Component, computed, inject, OnInit, signal } from '@angular/core';
import {
  ActivatedRoute,
  NavigationEnd,
  Router,
  RouterLink,
  RouterLinkActive,
  RouterOutlet,
} from '@angular/router';
import { filter } from 'rxjs';
import { Member } from '../../../types/member';
import { AgePipe } from '../../../core/pipes/age-pipe';
import { AccountService } from '../../../core/services/account-service';
import { MemberService } from '../../../core/services/member-service';
import { PresenceService } from '../../../core/services/presence-service';
import { LikesService } from '../../../core/services/likes-service';
import { BlocksService } from '../../../core/services/blocks-service';

@Component({
  selector: 'app-members-detailed',
  imports: [RouterLink, RouterLinkActive, RouterOutlet, AgePipe],
  templateUrl: './members-detailed.html',
  styleUrl: './members-detailed.css',
})
export class MembersDetailed implements OnInit {
  protected memberService = inject(MemberService);
  protected accountService = inject(AccountService);
  protected presenceService = inject(PresenceService); 
  protected likeService = inject(LikesService); 
  protected blocksServie = inject(BlocksService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  protected title = signal<string | undefined>('Profile');
  private routeId = signal<string | null>(null);
  protected isCurrentUser = computed(() => {
    return this.accountService.currentUser()?.id === this.routeId();
  });
  protected hasLiked = computed(() => this.likeService.likeIds().includes(this.routeId()!)); 
  protected isVip = computed(()=> this.accountService.currentUser()?.roles.includes("Vip"));
    

  constructor() {
    this.route.paramMap.subscribe(params => {
      this.routeId.set(params.get('id'));
    })
    
  }
  

  ngOnInit(): void {
    this.title.set(this.route.firstChild?.snapshot?.title);

    this.router.events.pipe(filter((event) => event instanceof NavigationEnd)).subscribe({
      next: () => {
        this.title.set(this.route.firstChild?.snapshot?.title);
      },
    });
  }

  blockUser():void{
    const targetMemberId = this.routeId();
    if(targetMemberId){
     this.blocksServie.block(targetMemberId, "oh yeah i am blocking");
    }
  }
}
