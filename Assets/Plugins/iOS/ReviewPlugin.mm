//  ReviewPlugin.mm
//  http://kan-kikuchi.hatenablog.com/entry/ReviewManager
//
//  Created by kan kikuchi on 2017.06.03.
//
//  Unity側とのレビュー関連で連携を行うプラグイン

#import <StoreKit/StoreKit.h>

//=================================================================================
//Unity側との連携
//=================================================================================


extern "C" {
    
    //iOSのバージョンを取得
    float _GetiOSVersion(){
        return [[[UIDevice currentDevice] systemVersion] floatValue];
    }
    
    //レビューを催促する
    void _RequestReview(){
        [SKReviewController requestReview];
    }
    
}
